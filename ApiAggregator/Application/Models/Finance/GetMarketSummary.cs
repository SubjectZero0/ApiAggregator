using System.Text.Json.Serialization;

namespace Application.Models.Finance
{
	public class GetMarketSummaryQuery
	{
		public OrderingOptions OrderingOptions { get; }
		public int NumberOfMarkets { get; }

		public GetMarketSummaryQuery(OrderingOptions orderingOptions, int numberOfMarkets = 50)
		{
			OrderingOptions = orderingOptions;
			NumberOfMarkets = numberOfMarkets;
		}
	}

	public class OrderingOptions
	{
		public FieldOrdering FieldOrdering { get; }
		public FieldToSort FieldToSort { get; }

		public OrderingOptions(FieldOrdering fieldOrdering = FieldOrdering.Descending, FieldToSort fieldToSort = FieldToSort.HighestPrice)
		{
			FieldOrdering = fieldOrdering;
			FieldToSort = fieldToSort;
		}
	}

	public class GetMarketSummaryResponse
	{
		public IReadOnlyCollection<MarketResponse> Markets { get; }

		public GetMarketSummaryResponse(IReadOnlyCollection<MarketResponse> markets)
		{
			Markets = markets;
		}
	}

	public class MarketResponse
	{
		public string Ticker { get; }

		public double ClosePrice { get; }

		public double HighPrice { get; }

		public double LowPrice { get; }

		public int? TransactionCount { get; }

		public double OpenPrice { get; }

		public double Volume { get; }

		public MarketResponse(string ticker, double closePrice, double highPrice, double lowPrice, int? transactionCount, double openPrice, double volume)
		{
			Ticker = ticker;
			ClosePrice = closePrice;
			HighPrice = highPrice;
			LowPrice = lowPrice;
			TransactionCount = transactionCount;
			OpenPrice = openPrice;
			Volume = volume;
		}
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum FieldOrdering
	{
		Ascending,
		Descending,
		None
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum FieldToSort
	{
		HighestPrice,
		Volume
	}
}