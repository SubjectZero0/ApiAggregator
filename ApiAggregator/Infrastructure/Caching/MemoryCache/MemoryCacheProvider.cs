using Application.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching.MemoryCache
{
	internal class MemoryCachingProvider : ICachingProvider
	{
		private readonly IMemoryCache _memoryCache;

		public MemoryCachingProvider(IMemoryCache memoryCache) => _memoryCache = memoryCache;

		public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, TimeSpan expiration)
		{
			return await _memoryCache.GetOrCreateAsync(key, async entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = expiration;
				return await factory();
			});
		}

		public Task RemoveAsync(string key)
		{
			_memoryCache.Remove(key);
			return Task.CompletedTask;
		}
	}
}