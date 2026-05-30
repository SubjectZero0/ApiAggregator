using Application.Models.News;
using FluentAssertions;
using Gateway.Clients;
using Gateway.Models;
using Gateway.Providers;
using Gateway.Utils.Mappers;
using Microsoft.Extensions.Logging;
using Moq;

namespace GatewayTests.Providers;

public class NewsProviderTests
{
	private readonly Mock<INewsClient> _newsClient = new();
	private readonly Mock<IMapper<Article[], ArticleResponse[]>> _mapper = new();
	private readonly Mock<ILogger<NewsProvider>> _logger = new();
	private readonly NewsProvider _sut;

	public NewsProviderTests()
	{
		_sut = new NewsProvider(_newsClient.Object, _logger.Object, _mapper.Object);
	}

	[Fact]
	public async Task GetHeadlines_WhenClientReturnsNull_ReturnsNull()
	{
		//Arrange
		var query = new GetTopHeadlinesQuery();

		_newsClient.Setup(c => c.GetTopHeadlines(query)).ReturnsAsync((NewsHeadlines?)null);

		//Act
		var result = await _sut.GetHeadlines(query);

		//Assert
		result.Should().BeNull();
	}

	[Fact]
	public async Task GetHeadlines_WhenNotOrderedByDate_MapsArticlesAsIs()
	{
		//Arrange
		var articles = new[] { BuildArticle(DateTime.UtcNow), BuildArticle(DateTime.UtcNow.AddHours(-1)) };
		var headlines = new NewsHeadlines { Articles = articles };
		var query = new GetTopHeadlinesQuery(isOrderedByDate: false);
		var mappedArticles = new ArticleResponse[0];

		_newsClient
			.Setup(c => c.GetTopHeadlines(query))
			.ReturnsAsync(headlines);

		_mapper
			.Setup(m => m.Map(articles))
			.Returns(mappedArticles);

		//Act
		var result = await _sut.GetHeadlines(query);

		//Assert
		result.Should().NotBeNull();
		_mapper.Verify(m => m.Map(articles), Times.Once);
	}

	[Fact]
	public async Task GetHeadlines_WhenOrderedByDate_OrdersDescendingBeforeMapping()
	{
		//Arrange
		var older = BuildArticle(DateTime.UtcNow.AddHours(-2));
		var newer = BuildArticle(DateTime.UtcNow);
		var articles = new[] { older, newer };
		var headlines = new NewsHeadlines { Articles = articles };
		var query = new GetTopHeadlinesQuery(isOrderedByDate: true);
		var mappedArticles = Array.Empty<ArticleResponse>();

		_newsClient
			.Setup(c => c.GetTopHeadlines(query))
			.ReturnsAsync(headlines);

		_mapper
			.Setup(m => m.Map(It.Is<Article[]>(a => a[0] == newer && a[1] == older)))
			.Returns(mappedArticles);

		//Act
		var result = await _sut.GetHeadlines(query);

		//Assert
		result.Should().NotBeNull();

		_mapper.Verify(
			m => m.Map(It.Is<Article[]>(a => a[0] == newer && a[1] == older)),
			Times.Once);
	}

	private static Article BuildArticle(DateTime publishedAt)
	{
		var article = new Article();

		var json = $$"""{"publishedAt":"{{publishedAt:o}}"}""";

		return System.Text.Json.JsonSerializer.Deserialize<Article>(json)!;
	}
}