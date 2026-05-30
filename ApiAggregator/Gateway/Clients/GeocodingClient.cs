using ApiAggregator.Configurations;
using Gateway.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gateway.Clients
{
	public interface IGeocodingClient
	{
		Task<Geocoding?> GetGeocoding(string cityName, string countryCode);
	}

	internal class GeocodingClient : IGeocodingClient
	{
		private readonly HttpClient _httpClient;
		private readonly GeocodingApiConfiguration _geocodingCfg;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly ILogger<GeocodingClient> _logger;
		private const int _limit = 1;

		public GeocodingClient(HttpClient httpClient, IOptions<GeocodingApiConfiguration> geocodingOptions, ILogger<GeocodingClient> logger)
		{
			_httpClient = httpClient;
			_httpClient.DefaultRequestHeaders.Add("User-Agent", "ApiAggregatorApp");

			_geocodingCfg = geocodingOptions.Value;

			_serializerOptions = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			_logger = logger;
		}

		public async Task<Geocoding?> GetGeocoding(string cityName, string countryCode)
		{
			var url = _geocodingCfg.BaseUrl + $"direct?q={cityName},{countryCode}&limit={_limit}&appid={_geocodingCfg.ApiKey}";

			try
			{
				var result = await _httpClient.GetFromJsonAsync<Geocoding[]>(url, _serializerOptions);

				return result?.FirstOrDefault();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return null;
			}
		}
	}
}