using Application.Models.News;
using Application.Providers;
using Microsoft.Extensions.Logging;

namespace Application.Services.Dashboard.News
{
	public interface IGetTopHeadlinesService
	{
		Task<GetTopHeadlinesResponse?> Get(GetTopHeadlinesQuery query);
	}

	internal class GetTopHeadlinesService : IGetTopHeadlinesService
	{
		private readonly INewsProvider _newsProvider;
		private readonly ILogger<GetTopHeadlinesService> _logger;

		public GetTopHeadlinesService(INewsProvider newsProvider, ILogger<GetTopHeadlinesService> logger)
		{
			_newsProvider = newsProvider;
			_logger = logger;
		}

		public async Task<GetTopHeadlinesResponse?> Get(GetTopHeadlinesQuery query)
		{
			var headlines = await _newsProvider.GetHeadlines(query);

			if (headlines is null)
				_logger.LogError("Could not retrieve headlines.");

			return headlines;
		}
	}
}