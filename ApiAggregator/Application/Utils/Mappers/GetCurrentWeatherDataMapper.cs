using Application.Models.Weather;
using Gateway.Models;

namespace Application.Utils.Mappers
{
	internal class GetCurrentWeatherDataMapper : IMapper<CurrentWeatherData, CurrentWeatherResponse>
	{
		public CurrentWeatherResponse Map(CurrentWeatherData input)
		{
			var weatherResponses = input.Weather
				.Select(weather => new WeatherResponse(weather.Main, weather.Description))
				.ToArray();

			var mainProps = new MainWeatherPropsResponse(
				temp: input.MainWeatherProps.Temp,
				feelsLike: input.MainWeatherProps.FeelsLike,
				tempMin: input.MainWeatherProps.TempMin,
				tempMax: input.MainWeatherProps.TempMax,
				pressure: input.MainWeatherProps.Pressure,
				humidity: input.MainWeatherProps.Humidity,
				seaLevel: input.MainWeatherProps.SeaLevel,
				groundLevel: input.MainWeatherProps.GroundLevel
			);

			var wind = new WindResponse(
				speed: input.Wind.Speed,
				deg: input.Wind.Deg,
				gust: input.Wind.Gust
			);

			return new CurrentWeatherResponse(
				weather: weatherResponses,
				mainWeatherProps: mainProps,
				wind: wind,
				visibility: input.Visibility
			);
		}
	}
}