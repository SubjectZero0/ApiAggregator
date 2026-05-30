using Application.Caching;
using Application.Models.Finance;
using Application.Models.News;
using Application.Providers;
using FluentAssertions;
using Infrastructure.Caching.Decorators;
using Moq;
using static Common.Constants;

namespace InfrastructureTests.Caching.Decorators;

public class MarketCacheDecoratorTests
{
	private readonly Mock<IMarketProvider> _innerProvider = new();
	private readonly Mock<ICachingProvider> _cachingProvider = new();
	private readonly MarketCacheDecorator _sut;

	public MarketCacheDecoratorTests()
	{
		_sut = new MarketCacheDecorator(_innerProvider.Object, _cachingProvider.Object);
	}

	[Fact]
	public async Task GetMarkets_CallsCachingProviderWithCorrectCacheName()
	{
		//Arrange
		var query = new GetMarketSummaryQuery(new OrderingOptions(FieldOrdering.Descending, FieldToSort.HighestPrice), 10);

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				It.IsAny<string>(),
				It.IsAny<Func<Task<GetMarketSummaryResponse?>>>(),
				CacheName.Finance))
			.ReturnsAsync((GetMarketSummaryResponse?)null);

		//Act
		await _sut.GetMarkets(query);

		//Assert
		_cachingProvider.Verify(c => c.GetOrSetAsync(
			It.IsAny<string>(),
			It.IsAny<Func<Task<GetMarketSummaryResponse?>>>(),
			CacheName.Finance), Times.Once);
	}

	[Fact]
	public async Task GetMarkets_BuildsCacheKeyFromQueryParameters()
	{
		//Arrange
		var query = new GetMarketSummaryQuery(new OrderingOptions(FieldOrdering.Ascending, FieldToSort.Volume), 25);
		var expectedKey = $"{CacheName.Finance}_25_{FieldToSort.Volume}_{FieldOrdering.Ascending}";

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				expectedKey,
				It.IsAny<Func<Task<GetMarketSummaryResponse?>>>(),
				CacheName.Finance))
			.ReturnsAsync((GetMarketSummaryResponse?)null);

		//Act
		await _sut.GetMarkets(query);

		//Assert
		_cachingProvider.Verify(c => c.GetOrSetAsync(
			expectedKey,
			It.IsAny<Func<Task<GetMarketSummaryResponse?>>>(),
			CacheName.Finance), Times.Once);
	}

	[Fact]
	public async Task GetMarkets_ReturnsCachedResult()
	{
		//Arrange
		var query = new GetMarketSummaryQuery(new OrderingOptions());
		var cached = new GetMarketSummaryResponse([]);

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				It.IsAny<string>(),
				It.IsAny<Func<Task<GetMarketSummaryResponse?>>>(),
				It.IsAny<string>()))
			.ReturnsAsync(cached);

		//Act
		var result = await _sut.GetMarkets(query);

		//Assert
		result.Should().BeSameAs(cached);
		_innerProvider.Verify(p => p.GetMarkets(It.IsAny<GetMarketSummaryQuery>()), Times.Never);
	}
}