using Application.Models.Metrics;

namespace Application.Services.Metrics
{
	public interface IMetricsService
	{
		IReadOnlyCollection<OutgoingApiCallMetrics> GetOutgoingApiCallMetrics();
	}

	internal class MetricsService : IMetricsService
	{
		public IReadOnlyCollection<OutgoingApiCallMetrics> GetOutgoingApiCallMetrics()
		{
			var metrics = OutgoingApiCallMetricsHandler.Records
				.GroupBy(record => record.Api)
				.Select(grp =>
				{
					var times = grp.Select(x => x.Duration).ToList();
					var total = times.Count;

					return new OutgoingApiCallMetrics(
						api: grp.Key,
						totalRequests: total,
						averageResponseTimeMs: total > 0 ? (decimal)times.Average() : 0,
						performanceBuckets: new PerformanceBucket(
							fastCount: times.Count(t => t < 200),
							average: times.Count(t => t >= 200 && t <= 500),
							slow: times.Count(t => t > 500)));
				});

			return metrics.ToArray();
		}
	}
}