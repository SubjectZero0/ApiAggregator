using Application.Models.Finance;
using Application.Providers;
using Application.Services.Dashboard.Finance;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationTests.Services.Dashboard.Finance;

public class GetMarketSummaryServiceTests
{
	private readonly Mock<IMarketProvider> _marketProvider = new();
	private readonly Mock<ILogger<GetMarketSummaryService>> _logger = new();
	private readonly IGetMarketSummaryService _sut;

	public GetMarketSummaryServiceTests()
	{
		_sut = new GetMarketSummaryService(_marketProvider.Object, _logger.Object);
	}

	[Fact]
	public async Task Get_WhenProviderReturnsResponse_ReturnsIt()
	{
		//Arrange
		var query = new GetMarketSummaryQuery(new OrderingOptions());

		var expected = new GetMarketSummaryResponse(
			[new MarketResponse("AAPL", 150, 155, 148, 1000, 149, 500000)]);

		_marketProvider
			.Setup(p => p.GetMarkets(query))
			.ReturnsAsync(expected);

		//Act
		var result = await _sut.Get(query);

		//Assert
		result.Should().BeSameAs(expected);
	}

	[Fact]
	public async Task Get_WhenProviderReturnsNull_ReturnsNull()
	{
		//Arrange
		var query = new GetMarketSummaryQuery(new OrderingOptions());

		_marketProvider
			.Setup(p => p.GetMarkets(query))
			.ReturnsAsync((GetMarketSummaryResponse?)null);

		//Act
		var result = await _sut.Get(query);

		//Assert
		result.Should().BeNull();
	}
}