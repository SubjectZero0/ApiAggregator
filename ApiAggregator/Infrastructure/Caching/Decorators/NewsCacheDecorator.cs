using Application.Caching;
using Application.Models.News;
using Application.Providers;
using static Common.Constants;

namespace Infrastructure.Caching.Decorators
{
	internal class NewsCacheDecorator : INewsProvider
	{
		private readonly INewsProvider _newsProvider;
		private readonly ICachingProvider _cachingProvider;

		public NewsCacheDecorator(INewsProvider newsProvider, ICachingProvider cachingProvider)
		{
			_newsProvider = newsProvider;
			_cachingProvider = cachingProvider;
		}

		public async Task<GetTopHeadlinesResponse?> GetHeadlines(GetTopHeadlinesQuery query)
		{
			return await _cachingProvider.GetOrSetAsync(
				key: GetCacheKey(query),
				factory: () => _newsProvider.GetHeadlines(query),
				cacheName: CacheName.News);
		}

		private static string GetCacheKey(GetTopHeadlinesQuery query)
			=> CacheName.News + "_" + $"{query.Category}_{query.PageSize}";
	}
}