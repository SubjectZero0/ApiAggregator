using Application.Models.News;
using Application.Providers;
using Application.Services.Dashboard.News;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationTests.Services.Dashboard.News;

public class GetTopHeadlinesServiceTests
{
	private readonly Mock<INewsProvider> _newsProvider = new();
	private readonly Mock<ILogger<GetTopHeadlinesService>> _logger = new();
	private readonly IGetTopHeadlinesService _sut;

	public GetTopHeadlinesServiceTests()
	{
		_sut = new GetTopHeadlinesService(_newsProvider.Object, _logger.Object);
	}

	[Fact]
	public async Task Get_WhenProviderReturnsResponse_ReturnsIt()
	{
		//Arrange
		var query = new GetTopHeadlinesQuery();

		var expected = new GetTopHeadlinesResponse([]);

		_newsProvider
			.Setup(p => p.GetHeadlines(query))
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
		var query = new GetTopHeadlinesQuery();

		_newsProvider
			.Setup(p => p.GetHeadlines(query))
			.ReturnsAsync((GetTopHeadlinesResponse?)null);

		//Act
		var result = await _sut.Get(query);

		//Assert
		result.Should().BeNull();
	}
}