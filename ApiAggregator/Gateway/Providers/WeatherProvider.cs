using Application.Models.Weather;
using Application.Providers;
using Gateway.Clients;
using Gateway.Models;
using Gateway.Utils.Mappers;

namespace Gateway.Providers
{
	internal class WeatherProvider : IWeatherProvider
	{
		private readonly IGeocodingClient _geocodingClient;
		private readonly IWeatherClient _weatherClient;
		private readonly IMapper<CurrentWeatherData, CurrentWeatherResponse> _mapper;

		public WeatherProvider(IGeocodingClient geocodingClient, IWeatherClient weatherClient, IMapper<CurrentWeatherData, CurrentWeatherResponse> mapper)
		{
			_geocodingClient = geocodingClient;
			_weatherClient = weatherClient;
			_mapper = mapper;
		}

		public async Task<CurrentWeatherResponse?> GetWeather(GetCurrentWeatherQuery query)
		{
			var geoData = await _geocodingClient.GetGeocoding(
				cityName: query.CityName,
				countryCode: query.CountryCode);

			if (geoData is null)
				return null;

			var currentWeather = await _weatherClient.GetCurrentWeather(
				latitude: geoData.Latitude,
				longitude: geoData.Longitude);

			if (currentWeather is null)
				return null;

			return _mapper.Map(currentWeather);
		}
	}
}