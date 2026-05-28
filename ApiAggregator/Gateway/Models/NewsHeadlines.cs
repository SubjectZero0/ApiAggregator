using System.Text.Json.Serialization;

namespace Gateway.Models
{
	public class NewsHeadlines
	{
		public string Status { get; set; } = string.Empty;

		public int TotalResults { get; set; }

		public IReadOnlyCollection<Article> Articles { get; set; } = [];
	}

	public class Article
	{
		[JsonInclude]
		[JsonPropertyName("publishedAt")]
		private string _publishedAtRaw
		{
			get => PublishedAtUtc.ToString("o");
			set
			{
				PublishedAtUtc = DateTime.TryParse(value, out var parsedDate)
					? parsedDate
					: DateTime.MinValue;
			}
		}

		public Source? Source { get; set; } = new();

		public string Author { get; set; } = string.Empty;

		public string Title { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public string Url { get; set; } = string.Empty;

		public string UrlToImage { get; set; } = string.Empty;

		[JsonIgnore]
		public DateTime PublishedAtUtc { get; private set; }

		public string Content { get; set; } = string.Empty;
	}

	public class Source
	{
		public string? Id { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}