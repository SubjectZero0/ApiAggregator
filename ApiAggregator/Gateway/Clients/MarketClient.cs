using ApiAggregator.Configurations;
using Gateway.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Gateway.Clients
{
	public interface IMarketClient
	{
		Task<DailyMakretSummary?> GetDailyMarketSummary(string date);
	}

	internal class MarketClient : IMarketClient
	{
		private readonly HttpClient _httpClient;
		private readonly MassiveApiConfiguration _massiveCfg;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly ILogger<MarketClient> _logger;

		public MarketClient(HttpClient httpClient, IOptions<MassiveApiConfiguration> massiveOptions, ILogger<MarketClient> logger)
		{
			_httpClient = httpClient;
			_httpClient.DefaultRequestHeaders.Add("User-Agent", "ApiAggregatorApp");

			_massiveCfg = massiveOptions.Value;

			_serializerOptions = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = false,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			_logger = logger;
		}

		public async Task<DailyMakretSummary?> GetDailyMarketSummary(string date)
		{
			var url = _massiveCfg.BaseUrl + $"aggs/grouped/locale/us/market/stocks/{date}?adjusted=true&include_otc=false&apiKey={_massiveCfg.ApiKey}";

			try
			{
				var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

				response.EnsureSuccessStatusCode();

				using var stream = await response.Content.ReadAsStreamAsync();

				return await JsonSerializer.DeserializeAsync<DailyMakretSummary>(stream, _serializerOptions);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return null;
			}
		}
	}
}