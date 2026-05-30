using Application.Caching;
using Microsoft.Extensions.Caching.Hybrid;

namespace Infrastructure.Caching.HybridCaching
{
	internal class HybridCacheProvider : ICachingProvider
	{
		private readonly HybridCache _cache;
		private readonly IHybridCacheOptionsFactory _optionsFactory;

		public HybridCacheProvider(HybridCache cache, IHybridCacheOptionsFactory optionsFactory)
		{
			_cache = cache;
			_optionsFactory = optionsFactory;
		}

		public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, string cacheName)
		{
			return await _cache.GetOrCreateAsync(
				key: key,
				factory: async token => await factory(),
				options: _optionsFactory.GetOptions(cacheName));
		}

		public async Task RemoveAsync(string key)
		{
			await _cache.RemoveAsync(key);
		}
	}
}