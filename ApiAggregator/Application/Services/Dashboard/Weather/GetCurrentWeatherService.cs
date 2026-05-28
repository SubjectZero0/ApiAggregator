using Application.Models.Weather;
using Application.Utils.Mappers;
using Gateway.Clients;
using Gateway.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Dashboard.Weather
{
	public interface IGetCurrentWeatherService
	{
		Task<CurrentWeatherResponse?> Get(GetCurrentWeatherQuery query);
	}

	internal class GetCurrentWeatherService : IGetCurrentWeatherService
	{
		private readonly IWeatherClient _weatherClient;
		private readonly IGeocodingClient _geocodingClient;
		private readonly IMapper<CurrentWeatherData, CurrentWeatherResponse> _mapper;
		private readonly ILogger<GetCurrentWeatherService> _logger;

		public GetCurrentWeatherService(IWeatherClient weatherClient, IGeocodingClient geocodingClient, ILogger<GetCurrentWeatherService> logger, IMapper<CurrentWeatherData, CurrentWeatherResponse> mapper)
		{
			_weatherClient = weatherClient;
			_geocodingClient = geocodingClient;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<CurrentWeatherResponse?> Get(GetCurrentWeatherQuery query)
		{
			var geoData = await _geocodingClient.GetGeocoding(query.CityName, query.CountryCode);

			if (geoData is null)
			{
				_logger.LogError("City {CityName} with Code: {CountryCode} not found.", query.CityName, query.CountryCode);
				return null;
			}

			var currentWeather = await _weatherClient.GetCurrentWeather(geoData.Latitude, geoData.Longitude);

			if (currentWeather is null)
			{
				_logger.LogError("No current weather data found for City: {CityName} with Code: {CountryCode}.", query.CityName, query.CountryCode);
				return null;
			}

			return _mapper.Map(currentWeather);
		}
	}
}