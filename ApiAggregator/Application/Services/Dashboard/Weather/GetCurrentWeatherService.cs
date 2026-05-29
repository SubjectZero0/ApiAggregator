using Application.Models.Weather;
using Application.Providers;
using Microsoft.Extensions.Logging;

namespace Application.Services.Dashboard.Weather
{
	public interface IGetCurrentWeatherService
	{
		Task<CurrentWeatherResponse?> Get(GetCurrentWeatherQuery query);
	}

	internal class GetCurrentWeatherService : IGetCurrentWeatherService
	{
		private readonly IWeatherProvider _weatherProvider;
		private readonly ILogger<GetCurrentWeatherService> _logger;

		public GetCurrentWeatherService(IWeatherProvider weatherProvider, ILogger<GetCurrentWeatherService> logger)
		{
			_weatherProvider = weatherProvider;
			_logger = logger;
		}

		public async Task<CurrentWeatherResponse?> Get(GetCurrentWeatherQuery query)
		{
			var currentWeather = await _weatherProvider.GetWeather(query);

			if (currentWeather is null)
				_logger.LogError("Could not retrieve current weather for {CityName}, {CountryCode}", query.CityName, query.CountryCode);

			return currentWeather;
		}
	}
}