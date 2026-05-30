namespace Application.Models.Metrics
{
	public class OutgoingApiCallMetrics
	{
		public string Api { get; }

		public int TotalRequests { get; }

		public decimal AverageResponseTimeMs { get; }

		public PerformanceBucket PerformanceBuckets { get; }

		public OutgoingApiCallMetrics(string api, int totalRequests, decimal averageResponseTimeMs, PerformanceBucket performanceBuckets)
		{
			Api = api;
			TotalRequests = totalRequests;
			AverageResponseTimeMs = averageResponseTimeMs;
			PerformanceBuckets = performanceBuckets;
		}
	}

	public class PerformanceBucket
	{
		public int FastCount { get; }
		public int Average { get; }
		public int Slow { get; }

		public PerformanceBucket(int fastCount, int average, int slow)
		{
			FastCount = fastCount;
			Average = average;
			Slow = slow;
		}
	}
}