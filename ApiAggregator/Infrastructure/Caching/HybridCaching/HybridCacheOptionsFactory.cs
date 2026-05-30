using Application.Configurations;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using static Common.Constants;

namespace Infrastructure.Caching.HybridCaching
{
	internal interface IHybridCacheOptionsFactory
	{
		HybridCacheEntryOptions GetOptions(string cacheName);
	}

	internal class HybridCacheOptionsFactory : IHybridCacheOptionsFactory
	{
		private readonly Dictionary<string, HybridCacheEntryOptions> _cacheOptions;

		public HybridCacheOptionsFactory(IOptions<CachingConfiguration> cacheCfg)
		{
			var cacheConfiguration = cacheCfg.Value;

			_cacheOptions = new Dictionary<string, HybridCacheEntryOptions>
			{
				{ CacheName.News, new HybridCacheEntryOptions { Expiration = cacheConfiguration.NewsExpiry } },
				{ CacheName.Weather, new HybridCacheEntryOptions { Expiration = cacheConfiguration.WeatherExpiry } },
				{ CacheName.Finance, new HybridCacheEntryOptions { Expiration = cacheConfiguration.MarketExpiry } }
			};
		}

		public HybridCacheEntryOptions GetOptions(string cacheName)
		{
			if (cacheName is CacheName.DailyMarkets)
			{
				return new HybridCacheEntryOptions
				{
					Expiration = GetTimeUntilMidnight(),
				};
			}

			if (!_cacheOptions.TryGetValue(cacheName, out var options))
				throw new ArgumentException("Cache with name {CacheName} was not found.", cacheName);

			return options;
		}

		private static TimeSpan GetTimeUntilMidnight()
		{
			var now = DateTime.UtcNow;
			var midnight = now.Date.AddDays(1);

			return midnight - now;
		}
	}
}