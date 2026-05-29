using Application.Models.News;
using Application.Providers;
using Gateway.Clients;
using Gateway.Models;
using Gateway.Utils.Mappers;
using Microsoft.Extensions.Logging;

namespace Gateway.Providers
{
	internal class NewsProvider : INewsProvider
	{
		private readonly INewsClient _newsClient;
		private readonly IMapper<Article[], ArticleResponse[]> _mapper;
		private readonly ILogger<NewsProvider> _logger;

		public NewsProvider(INewsClient newsClient, ILogger<NewsProvider> logger, IMapper<Article[], ArticleResponse[]> mapper)
		{
			_newsClient = newsClient;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<GetTopHeadlinesResponse?> GetHeadlines(GetTopHeadlinesQuery query)
		{
			var headlines = await _newsClient.GetTopHeadlines(query);

			if (headlines is null)
			{
				_logger.LogError("No Headlines found for Category: {Category}", query.Category);
				return null;
			}

			if (query.IsOrderedByDate)
			{
				var orderedArticles = headlines.Articles
					.OrderByDescending(x => x.PublishedAtUtc)
					.ToArray();

				return new GetTopHeadlinesResponse(articles: _mapper.Map(orderedArticles));
			}

			return new GetTopHeadlinesResponse(articles: _mapper.Map(headlines.Articles.ToArray()));
		}
	}
}