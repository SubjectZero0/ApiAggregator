using Application.Caching;
using Microsoft.Extensions.Caching.Hybrid;

namespace Infrastructure.Caching.HybridCacheProvider
{
	internal class HybridCacheProvider : ICachingProvider
	{
		private readonly HybridCache _cache;

		public HybridCacheProvider(HybridCache cache)
		{
			_cache = cache;
		}

		public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, TimeSpan expiration)
		{
			return await _cache.GetOrCreateAsync(
				key: key,
				factory: async token => await factory(),
				options: new HybridCacheEntryOptions() { Expiration = expiration });
		}

		public async Task RemoveAsync(string key)
		{
			await _cache.RemoveAsync(key);
		}
	}
}