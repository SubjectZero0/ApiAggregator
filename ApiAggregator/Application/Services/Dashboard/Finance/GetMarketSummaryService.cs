using Application.Models.Finance;
using Application.Providers;
using Microsoft.Extensions.Logging;

namespace Application.Services.Dashboard.Finance
{
	public interface IGetMarketSummaryService
	{
		Task<GetMarketSummaryResponse?> Get(GetMarketSummaryQuery query);
	}

	internal class GetMarketSummaryService : IGetMarketSummaryService
	{
		private readonly IMarketProvider _marketProvider;
		private readonly ILogger<GetMarketSummaryService> _logger;

		public GetMarketSummaryService(IMarketProvider marketProvider, ILogger<GetMarketSummaryService> logger)
		{
			_marketProvider = marketProvider;
			_logger = logger;
		}

		public async Task<GetMarketSummaryResponse?> Get(GetMarketSummaryQuery query)
		{
			var markets = await _marketProvider.GetMarkets(query);

			if (markets is null)
				_logger.LogError("Could not retrieve markets.");

			return markets;
		}
	}
}