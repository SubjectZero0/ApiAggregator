using System.Text.Json.Serialization;

namespace Gateway.Models
{
	public class DailyMakretSummary
	{
		public bool Adjusted { get; set; }

		public int QueryCount { get; set; }

		[JsonPropertyName("request_id")]
		public string RequestId { get; set; } = string.Empty;

		public int ResultsCount { get; set; }

		public string Status { get; set; } = string.Empty;

		public IReadOnlyCollection<MassiveStockTicker> Results { get; set; } = [];
	}

	public class MassiveStockTicker
	{
		[JsonPropertyName("T")]
		public string Ticker { get; set; } = string.Empty;

		[JsonPropertyName("c")]
		public double ClosePrice { get; set; }

		[JsonPropertyName("h")]
		public double HighPrice { get; set; }

		[JsonPropertyName("l")]
		public double LowPrice { get; set; }

		[JsonPropertyName("n")]
		public int? TransactionCount { get; set; }

		[JsonPropertyName("o")]
		public double OpenPrice { get; set; }

		[JsonPropertyName("otc")]
		public bool? IsOtc { get; set; }

		[JsonPropertyName("v")]
		public double Volume { get; set; }

		[JsonPropertyName("vw")]
		public double? VolumeWeightedAveragePrice { get; set; }
	}
}