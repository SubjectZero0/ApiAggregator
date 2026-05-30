using Application.Caching;
using Application.Models.Finance;
using Application.Providers;
using static Common.Constants;

namespace Infrastructure.Caching.Decorators
{
	internal class MarketCacheDecorator : IMarketProvider
	{
		private readonly IMarketProvider _marketProvider;
		private readonly ICachingProvider _cachingProvider;

		public MarketCacheDecorator(IMarketProvider marketProvider, ICachingProvider cachingProvider)
		{
			_marketProvider = marketProvider;
			_cachingProvider = cachingProvider;
		}

		public async Task<GetMarketSummaryResponse?> GetMarkets(GetMarketSummaryQuery query)
		{
			return await _cachingProvider.GetOrSetAsync(
				key: GetCacheKey(query),
				factory: () => _marketProvider.GetMarkets(query),
				cacheName: CacheName.Finance);
		}

		private static string GetCacheKey(GetMarketSummaryQuery query)
			=> CacheName.Finance + "_" + $"{query.NumberOfMarkets}_{query.OrderingOptions.FieldToSort}_{query.OrderingOptions.FieldOrdering}";
	}
}