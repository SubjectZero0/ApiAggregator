using static Common.Constants;

namespace Application.Models.News
{
	public class GetTopHeadlinesQuery
	{
		public string Category { get; }

		public int PageSize { get; }

		public bool IsOrderedByDate { get; }

		public GetTopHeadlinesQuery()
		{
			Category = string.Empty;
		}

		public GetTopHeadlinesQuery(string category = NewsCategory.General, int pageSize = 20, bool isOrderedByDate = false)
		{
			Category = category;
			PageSize = pageSize;
			IsOrderedByDate = isOrderedByDate;
		}
	}

	public class GetTopHeadlinesResponse
	{
		public IReadOnlyCollection<ArticleResponse> Articles { get; }

		public GetTopHeadlinesResponse(IReadOnlyCollection<ArticleResponse> articles)
		{
			Articles = articles;
		}
	}

	public class ArticleResponse
	{
		public string SourceName { get; }

		public string Author { get; }

		public string Title { get; }

		public string Description { get; }

		public string Url { get; }

		public string UrlToImage { get; }

		public DateTime PublishedAtUtc { get; }

		public string Content { get; }

		public ArticleResponse(
			string sourceName,
			string author,
			string title,
			string description,
			string url,
			string urlToImage,
			DateTime publishedAtUtc,
			string content)
		{
			SourceName = sourceName;
			Author = author;
			Title = title;
			Description = description;
			Url = url;
			UrlToImage = urlToImage;
			PublishedAtUtc = publishedAtUtc;
			Content = content;
		}
	}
}