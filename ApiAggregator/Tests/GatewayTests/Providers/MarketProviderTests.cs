using Application.Caching;
using Application.Models.Finance;
using FluentAssertions;
using Gateway.Clients;
using Gateway.Models;
using Gateway.Providers;
using Gateway.Utils;
using Gateway.Utils.Mappers;
using Microsoft.Extensions.Logging;
using Moq;

namespace GatewayTests.Providers;

public class MarketProviderTests
{
	private readonly Mock<IMarketClient> _marketClient = new();
	private readonly Mock<IMapper<MassiveStockTicker[], MarketResponse[]>> _mapper = new();
	private readonly Mock<IMarketSorter> _sorter = new();
	private readonly Mock<ICachingProvider> _cachingProvider = new();
	private readonly Mock<ILogger<MarketProvider>> _logger = new();
	private readonly MarketProvider _sut;

	public MarketProviderTests()
	{
		_sut = new MarketProvider(
			_marketClient.Object,
			_mapper.Object,
			_sorter.Object,
			_cachingProvider.Object,
			_logger.Object);
	}

	[Fact]
	public async Task GetMarkets_WhenCacheReturnsNull_ReturnsNull()
	{
		//Arrange
		var query = new GetMarketSummaryQuery(new OrderingOptions());

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				It.IsAny<string>(), It.IsAny<Func<Task<DailyMakretSummary?>>>(), It.IsAny<string>()))
			.ReturnsAsync((DailyMakretSummary?)null);

		//Act
		var result = await _sut.GetMarkets(query);

		//Assert
		result.Should().BeNull();
		_sorter.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task GetMarkets_WhenCacheReturnsData_SortsAndMapsResult()
	{
		//Arrange
		var query = new GetMarketSummaryQuery(
			new OrderingOptions(FieldOrdering.Descending, FieldToSort.HighestPrice),
			numberOfMarkets: 5);

		var ticker = new MassiveStockTicker { Ticker = "AAPL", HighPrice = 150, Volume = 100 };

		var dailySummary = new DailyMakretSummary { Results = [ticker] };

		var sortedTickers = new[] { ticker };

		var mappedMarkets = new[] { new MarketResponse("AAPL", 148, 150, 145, 1000, 147, 100) };

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				It.IsAny<string>(), It.IsAny<Func<Task<DailyMakretSummary?>>>(), It.IsAny<string>()))
			.ReturnsAsync(dailySummary);

		_sorter
			.Setup(s => s.Sort(
				It.IsAny<MassiveStockTicker[]>(),
				query.NumberOfMarkets,
				query.OrderingOptions.FieldOrdering,
				query.OrderingOptions.FieldToSort))
			.Returns(sortedTickers);

		_mapper
			.Setup(m => m.Map(sortedTickers))
			.Returns(mappedMarkets);

		//Act
		var result = await _sut.GetMarkets(query);

		//Assert
		result.Should().NotBeNull();
		result!.Markets.Should().BeEquivalentTo(mappedMarkets);
	}
}