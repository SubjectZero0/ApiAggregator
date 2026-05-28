using ApiAggregator.Configurations;
using Gateway.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gateway.Clients
{
	public interface IWeatherClient
	{
		Task<CurrentWeatherData?> GetCurrentWeather(double latitude, double longitude);
	}

	internal class WeatherClient : IWeatherClient
	{
		private readonly HttpClient _httpClient;
		private readonly WeatherApiConfiguration _weatherCfg;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly ILogger<WeatherClient> _logger;

		public WeatherClient(HttpClient httpClient, IOptions<WeatherApiConfiguration> weatherOptions, ILogger<WeatherClient> logger)
		{
			_httpClient = httpClient;
			_httpClient.DefaultRequestHeaders.Add("User-Agent", "ApiAggregatorApp");

			_weatherCfg = weatherOptions.Value;

			_serializerOptions = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			_logger = logger;
		}

		public async Task<CurrentWeatherData?> GetCurrentWeather(double latitude, double longitude)
		{
			var url = _weatherCfg.BaseUrl + $"weather?lat={latitude}&lon={longitude}&appid={_weatherCfg.ApiKey}";

			try
			{
				var result = await _httpClient.GetFromJsonAsync<CurrentWeatherData>(url, _serializerOptions);

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return null;
			}
		}
	}
}