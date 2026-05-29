using Application.Models.Finance;

namespace Application.Providers
{
	public interface IMarketProvider
	{
		Task<GetMarketSummaryResponse?> GetMarkets(GetMarketSummaryQuery query);
	}
}