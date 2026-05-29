using Application.Models;
using Application.Models.Finance;
using Application.Models.News;
using Application.Models.Weather;
using Application.Services.Dashboard.Finance;
using Application.Services.Dashboard.News;
using Application.Services.Dashboard.Weather;

namespace Application.Services.Dashboard
{
	public interface IDashBoardService
	{
		Task<DashBoardResponse> GetDashBoard(GetDashBoardQuery query);
	}

	internal class DashBoardService : IDashBoardService
	{
		private readonly IGetCurrentWeatherService _weatherService;
		private readonly IGetTopHeadlinesService _newsService;
		private readonly IGetMarketSummaryService _marketService;

		public DashBoardService(IGetCurrentWeatherService weatherService, IGetTopHeadlinesService newsService, IGetMarketSummaryService marketService)
		{
			_weatherService = weatherService;
			_newsService = newsService;
			_marketService = marketService;
		}

		public async Task<DashBoardResponse> GetDashBoard(GetDashBoardQuery query)
		{
			var weatherQuery = new GetCurrentWeatherQuery(
				cityName: query.CityName,
				countryCode: query.CountryCode);

			var newsQuery = new GetTopHeadlinesQuery(
				category: query.NewsCategory,
				pageSize: query.NewsPageSize,
				isOrderedByDate: query.IsOrderedByDate);

			var financeQuery = new GetMarketSummaryQuery(
				orderingOptions: query.OrderingOptions,
				numberOfMarkets: query.NumberOfMarkets);

			var weatherTask = _weatherService.Get(weatherQuery);
			var newsTask = _newsService.Get(newsQuery);
			var financeTask = _marketService.Get(financeQuery);

			await Task.WhenAll(weatherTask, newsTask, financeTask);

			var currentWeather = await weatherTask;
			var currentNews = await newsTask;
			var marketSummary = await financeTask;

			return new DashBoardResponse(currentWeather, currentNews, marketSummary);
		}
	}
}