using Application.Caching;
using Application.Models.Weather;
using Application.Providers;
using static Common.Constants;

namespace Infrastructure.Caching.Decorators
{
	internal class WeatherCacheDecorator : IWeatherProvider
	{
		private readonly IWeatherProvider _weatherProvider;
		private readonly ICachingProvider _cachingProvider;

		public WeatherCacheDecorator(IWeatherProvider weatherProvider, ICachingProvider cachingProvider)
		{
			_weatherProvider = weatherProvider;
			_cachingProvider = cachingProvider;
		}

		public async Task<CurrentWeatherResponse?> GetWeather(GetCurrentWeatherQuery query)
		{
			return await _cachingProvider.GetOrSetAsync(
				key: GetCacheKey(query),
				factory: () => _weatherProvider.GetWeather(query),
				cacheName: CacheName.Weather);
		}

		private static string GetCacheKey(GetCurrentWeatherQuery query)
			=> CacheName.Weather + "_" + $"{query.CityName.Trim().ToLowerInvariant()}_{query.CountryCode.Trim().ToLowerInvariant()}";
	}
}