using Application.Caching;
using Application.Configurations;
using Application.Models.Weather;
using Application.Providers;
using Microsoft.Extensions.Options;

namespace Infrastructure.Caching.Decorators
{
	internal class WeatherCacheDecorator : IWeatherProvider
	{
		private readonly IWeatherProvider _weatherProvider;
		private readonly ICachingProvider _cachingProvider;
		private readonly CachingConfiguration _cacheCfg;

		private const string _weatherCacheKeyBase = "weatherCache";

		public WeatherCacheDecorator(IWeatherProvider weatherProvider, ICachingProvider cachingProvider, IOptions<CachingConfiguration> cacheCfg)
		{
			_weatherProvider = weatherProvider;
			_cachingProvider = cachingProvider;
			_cacheCfg = cacheCfg.Value;
		}

		public async Task<CurrentWeatherResponse?> GetWeather(GetCurrentWeatherQuery query)
		{
			return await _cachingProvider.GetOrSetAsync(
				key: GetCacheKey(query),
				factory: () => _weatherProvider.GetWeather(query),
				expiration: _cacheCfg.WeatherExpiry);
		}

		private string GetCacheKey(GetCurrentWeatherQuery query)
		{
			return _weatherCacheKeyBase + "_" + $"{query.CityName}_{query.CountryCode}";
		}
	}
}