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

		public OrderingOptions OrderingOptions { get; init; }

		public int NumberOfMarkets { get; init; }

		public GetDashBoardQuery()
		{
			CityName = string.Empty;
			CountryCode = string.Empty;
			NewsCategory = string.Empty;
			OrderingOptions = new OrderingOptions();
		}

		public GetDashBoardQuery(string cityName, string countryCode, string newsCategory, int newsPageSize, bool isOrderedByDate, OrderingOptions orderingOptions, int numberOfMarkets)
		{
			CityName = cityName;
			CountryCode = countryCode;
			NewsCategory = newsCategory;
			NewsPageSize = newsPageSize;
			IsOrderedByDate = isOrderedByDate;
			OrderingOptions = orderingOptions;
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