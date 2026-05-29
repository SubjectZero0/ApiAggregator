using Application.Models.Weather;

namespace Application.Providers
{
	public interface IWeatherProvider
	{
		Task<CurrentWeatherResponse?> GetWeather(GetCurrentWeatherQuery query);
	}
}