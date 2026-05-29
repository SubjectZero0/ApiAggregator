using Application.Models.Finance;
using Gateway.Models;

namespace Gateway.Utils
{
	internal interface IMarketSorter
	{
		IReadOnlyCollection<MassiveStockTicker> Sort(MassiveStockTicker[] markets, int numberOfMarkets, FieldOrdering sortingDirection, FieldToSort fieldToSort);
	}

	internal class MarketSorter : IMarketSorter
	{
		public IReadOnlyCollection<MassiveStockTicker> Sort(MassiveStockTicker[] markets, int numberOfMarkets, FieldOrdering sortingDirection, FieldToSort fieldToSort)
		{
			if (sortingDirection is FieldOrdering.None)
				return markets
					.Take(numberOfMarkets)
					.ToArray();

			Func<MassiveStockTicker, double> sortedField = fieldToSort switch
			{
				FieldToSort.Volume => x => x.Volume,
				FieldToSort.HighestPrice => x => x.HighPrice,
				_ => x => x.HighPrice
			};

			var sortedMarkets = sortingDirection is FieldOrdering.Ascending
				? markets.OrderBy(sortedField).Take(numberOfMarkets)
				: markets.OrderByDescending(sortedField).Take(numberOfMarkets);

			return sortedMarkets.ToArray();
		}
	}
}