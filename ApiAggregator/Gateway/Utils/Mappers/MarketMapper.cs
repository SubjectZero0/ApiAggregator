using Application.Models.Finance;
using Gateway.Models;

namespace Gateway.Utils.Mappers
{
	internal class MarketMapper : IMapper<MassiveStockTicker[], MarketResponse[]>
	{
		public MarketResponse[] Map(MassiveStockTicker[] input)
		{
			var markets = input
				.Select(market => new MarketResponse(
					ticker: market.Ticker,
					closePrice: market.ClosePrice,
					highPrice: market.HighPrice,
					lowPrice: market.LowPrice,
					transactionCount: market.TransactionCount,
					openPrice: market.OpenPrice,
					volume: market.Volume))
				.ToArray();

			return markets;
		}
	}
}