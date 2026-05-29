using Application.Caching;
using Application.Configurations;
using Application.Models.Finance;
using Application.Providers;
using Microsoft.Extensions.Options;

namespace Infrastructure.Caching.Decorators
{
	internal class MarketCacheDecorator : IMarketProvider
	{
		private readonly IMarketProvider _marketProvider;
		private readonly ICachingProvider _cachingProvider;
		private readonly CachingConfiguration _cacheCfg;

		private const string _marketCacheKeyBase = "marketCache";

		public MarketCacheDecorator(IMarketProvider marketProvider, ICachingProvider cachingProvider, IOptions<CachingConfiguration> cacheCfg)
		{
			_marketProvider = marketProvider;
			_cachingProvider = cachingProvider;
			_cacheCfg = cacheCfg.Value;
		}

		public async Task<GetMarketSummaryResponse?> GetMarkets(GetMarketSummaryQuery query)
		{
			return await _cachingProvider.GetOrSetAsync(
				key: GetCacheKey(query),
				factory: () => _marketProvider.GetMarkets(query),
				expiration: _cacheCfg.MarketExpiry);
		}

		private string GetCacheKey(GetMarketSummaryQuery query)
		{
			return _marketCacheKeyBase + "-" + $"{query.NumberOfMarkets}-{query.OrderingOptions.FieldToSort}-{query.OrderingOptions.FieldOrdering}";
		}
	}
}