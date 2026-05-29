using Application.Models.Finance;
using Application.Models.News;
using Application.Models.Weather;

namespace Application.Models
{
	public class GetDashBoardQuery
	{
		public string CityName { get; init; }

		public string CountryCode { get; init; }

		public string NewsCategory { get; init; }

		public int NewsPageSize { get; init; }

		public bool IsOrderedByDate { get; init; }

		public OrderingOptions MarketOrderingOptions { get; init; }

		public int NumberOfMarkets { get; init; }

		public GetDashBoardQuery()
		{
			CityName = string.Empty;
			CountryCode = string.Empty;
			NewsCategory = string.Empty;
			MarketOrderingOptions = new OrderingOptions();
		}

		public GetDashBoardQuery(string cityName, string countryCode, string newsCategory, int newsPageSize, bool isOrderedByDate, OrderingOptions marketOrderingOptions, int numberOfMarkets)
		{
			CityName = cityName;
			CountryCode = countryCode;
			NewsCategory = newsCategory;
			NewsPageSize = newsPageSize;
			IsOrderedByDate = isOrderedByDate;
			MarketOrderingOptions = marketOrderingOptions;
			NumberOfMarkets = numberOfMarkets;
		}
	}

	public class DashBoardResponse
	{
		public CurrentWeatherResponse? WeatherResponse { get; }

		public GetTopHeadlinesResponse? TopHeadlinesResponse { get; }

		public GetMarketSummaryResponse? MarketSummaryResponse { get; }

		public DashBoardResponse()
		{
		}

		public DashBoardResponse(CurrentWeatherResponse? weatherResponse, GetTopHeadlinesResponse? topHeadlinesResponse, GetMarketSummaryResponse? marketSummaryResponse)
		{
			WeatherResponse = weatherResponse;
			TopHeadlinesResponse = topHeadlinesResponse;
			MarketSummaryResponse = marketSummaryResponse;
		}
	}
}