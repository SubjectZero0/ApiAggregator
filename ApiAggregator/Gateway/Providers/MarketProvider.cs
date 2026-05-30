using Application.Caching;
using Application.Models.Finance;
using Application.Providers;
using Gateway.Clients;
using Gateway.Models;
using Gateway.Utils;
using Gateway.Utils.Mappers;
using Microsoft.Extensions.Logging;
using System.Globalization;
using static Common.Constants;

namespace Gateway.Providers
{
	internal class MarketProvider : IMarketProvider
	{
		private readonly IMarketClient _marketClient;
		private readonly IMapper<MassiveStockTicker[], MarketResponse[]> _mapper;
		private readonly IMarketSorter _marketsorter;
		private readonly ICachingProvider _cachingProvider;
		private readonly ILogger<MarketProvider> _logger;

		public MarketProvider(IMarketClient marketClient, IMapper<MassiveStockTicker[], MarketResponse[]> mapper, IMarketSorter marketsorter, ICachingProvider cachingProvider, ILogger<MarketProvider> logger)
		{
			_marketClient = marketClient;
			_mapper = mapper;
			_marketsorter = marketsorter;
			_cachingProvider = cachingProvider;
			_logger = logger;
		}

		public async Task<GetMarketSummaryResponse?> GetMarkets(GetMarketSummaryQuery query)
		{
			var date = DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

			var markets = await _cachingProvider.GetOrSetAsync(
				key: CacheName.DailyMarkets,
				factory: () => _marketClient.GetDailyMarketSummary(date),
				cacheName: CacheName.DailyMarkets);

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