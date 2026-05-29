using Application.Models.Finance;
using Application.Providers;
using Gateway.Clients;
using Gateway.Models;
using Gateway.Utils;
using Gateway.Utils.Mappers;
using Microsoft.Extensions.Logging;

namespace Gateway.Providers
{
	internal class MarketProvider : IMarketProvider
	{
		private readonly IMarketClient _marketClient;
		private readonly IMapper<MassiveStockTicker[], MarketResponse[]> _mapper;
		private readonly IMarketSorter _marketsorter;
		private readonly ILogger<MarketProvider> _logger;

		public MarketProvider(IMarketClient marketClient, IMapper<MassiveStockTicker[], MarketResponse[]> mapper, IMarketSorter marketsorter, ILogger<MarketProvider> logger)
		{
			_marketClient = marketClient;
			_mapper = mapper;
			_marketsorter = marketsorter;
			_logger = logger;
		}

		public async Task<GetMarketSummaryResponse?> GetMarkets(GetMarketSummaryQuery query)
		{
			var markets = await _marketClient.GetDailyMarketSummary();

			if (markets is null)
			{
				_logger.LogError("No markets found.");
				return null;
			}

			var sortedMarkets = _marketsorter.Sort(
				markets: markets.Results.ToArray(),
				numberOfMarkets: query.NumberOfMarkets,
				sortingDirection: query.OrderingOptions.FieldOrdering,
				fieldToSort: query.OrderingOptions.FieldToSort);

			return new GetMarketSummaryResponse(markets: _mapper.Map(sortedMarkets.ToArray()));
		}
	}
}