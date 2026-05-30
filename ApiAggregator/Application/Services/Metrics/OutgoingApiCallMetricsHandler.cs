using System.Collections.Concurrent;
using System.Diagnostics;

namespace Application.Services.Metrics
{
	public class OutgoingApiCallMetricsHandler : DelegatingHandler
	{
		public static readonly ConcurrentBag<(string Api, double Duration)> Records = new();

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string apiName = request.RequestUri?.Host
				?? throw new Exception($"{nameof(OutgoingApiCallMetricsHandler)} - No Api Host found.");

			var sw = Stopwatch.StartNew();
			try
			{
				var response = await base.SendAsync(request, cancellationToken);

				if (response.Content != null)
				{
					await response.Content.LoadIntoBufferAsync();
				}

				return response;
			}
			finally
			{
				sw.Stop();
				Records.Add((apiName, sw.Elapsed.TotalMilliseconds));
			}
		}
	}
}