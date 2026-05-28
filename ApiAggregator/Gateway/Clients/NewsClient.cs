using ApiAggregator.Configurations;
using Gateway.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gateway.Clients
{
	public interface INewsClient
	{
		Task<NewsHeadlines?> GetTopHeadlines();
	}

	internal class NewsClient : INewsClient
	{
		private readonly HttpClient _httpClient;
		private readonly NewsApiConfiguration _newsCfg;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly ILogger<NewsClient> _logger;

		public NewsClient(HttpClient httpClient, IOptions<NewsApiConfiguration> newsOptions, ILogger<NewsClient> logger)
		{
			_httpClient = httpClient;
			_httpClient.DefaultRequestHeaders.Add("User-Agent", "ApiAggregatorApp");

			_newsCfg = newsOptions.Value;

			_serializerOptions = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			_logger = logger;
		}

		public async Task<NewsHeadlines?> GetTopHeadlines()
		{
			var url = _newsCfg.BaseUrl + $"top-headlines?country=us&category=business&apiKey={_newsCfg.ApiKey}";

			try
			{
				var result = await _httpClient.GetFromJsonAsync<NewsHeadlines?>(url, _serializerOptions);

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