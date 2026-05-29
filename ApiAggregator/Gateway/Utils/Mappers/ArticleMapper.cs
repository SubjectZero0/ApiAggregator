using Application.Models.News;
using Gateway.Models;

namespace Gateway.Utils.Mappers
{
	internal class ArticleMapper : IMapper<Article[], ArticleResponse[]>
	{
		public ArticleResponse[] Map(Article[] input)
		{
			var articles = input
				.Select(article => new ArticleResponse(
					sourceName: article.Source?.Name ?? string.Empty,
					author: article.Author,
					title: article.Title,
					description: article.Description,
					url: article.Url,
					urlToImage: article.UrlToImage,
					publishedAtUtc: article.PublishedAtUtc,
					content: article.Content))
				.ToArray();

			return articles;
		}
	}
}