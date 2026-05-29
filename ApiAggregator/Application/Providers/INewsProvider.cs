using Application.Models.News;

namespace Application.Providers
{
	public interface INewsProvider
	{
		Task<GetTopHeadlinesResponse?> GetHeadlines(GetTopHeadlinesQuery query);
	}
}