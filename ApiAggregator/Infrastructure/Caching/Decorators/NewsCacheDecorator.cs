using Application.Caching;
using Application.Configurations;
using Application.Models.News;
using Application.Providers;
using Microsoft.Extensions.Options;

namespace Infrastructure.Caching.Decorators
{
	internal class NewsCacheDecorator : INewsProvider
	{
		private readonly INewsProvider _newsProvider;
		private readonly ICachingProvider _cachingProvider;
		private readonly CachingConfiguration _cacheCfg;

		private const string _newsCacheKeyBase = "newsCache";

		public NewsCacheDecorator(INewsProvider newsProvider, ICachingProvider cachingProvider, IOptions<CachingConfiguration> cacheCfg)
		{
			_newsProvider = newsProvider;
			_cachingProvider = cachingProvider;
			_cacheCfg = cacheCfg.Value;
		}

		public async Task<GetTopHeadlinesResponse?> GetHeadlines(GetTopHeadlinesQuery query)
		{
			return await _cachingProvider.GetOrSetAsync(
				key: GetCacheKey(query),
				factory: () => _newsProvider.GetHeadlines(query),
				expiration: _cacheCfg.NewsExpiry);
		}

		private string GetCacheKey(GetTopHeadlinesQuery query)
		{
			return _newsCacheKeyBase + "_" + $"{query.Category}_{query.PageSize}";
		}
	}
}