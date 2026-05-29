using Application.Models.Finance;
using Application.Models.News;
using Application.Models.Weather;
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
	public class GetDashBoardQuery
	{
		[MaxLength(100, ErrorMessage = "City name is too long.")]
		public string CityName { get; init; }

		[MaxLength(2, ErrorMessage = "Country code is too long.")]
		public string CountryCode { get; init; }

		public string NewsCategory { get; init; }

		[Range(0, 50, ErrorMessage = "Number of articles must be between 0 and 50.")]
		public int NewsPageSize { get; init; }

		public bool IsOrderedByDate { get; init; }

		public OrderingOptions MarketOrderingOptions { get; init; }

		[Range(0, 100, ErrorMessage = "Number of markets must be between 0 and 100.")]
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