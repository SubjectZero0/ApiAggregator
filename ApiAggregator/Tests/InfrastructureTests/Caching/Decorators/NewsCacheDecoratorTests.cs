using Application.Caching;
using Application.Models.News;
using Application.Providers;
using FluentAssertions;
using Infrastructure.Caching.Decorators;
using Moq;
using static Common.Constants;

namespace InfrastructureTests.Caching.Decorators;

public class NewsCacheDecoratorTests
{
	private readonly Mock<INewsProvider> _innerProvider = new();
	private readonly Mock<ICachingProvider> _cachingProvider = new();
	private readonly NewsCacheDecorator _sut;

	public NewsCacheDecoratorTests()
	{
		_sut = new NewsCacheDecorator(_innerProvider.Object, _cachingProvider.Object);
	}

	[Fact]
	public async Task GetHeadlines_CallsCachingProviderWithCorrectCacheName()
	{
		//Arrange
		var query = new GetTopHeadlinesQuery(NewsCategory.Technology, 10);

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				It.IsAny<string>(),
				It.IsAny<Func<Task<GetTopHeadlinesResponse?>>>(),
				CacheName.News))
			.ReturnsAsync((GetTopHeadlinesResponse?)null);

		//Act
		await _sut.GetHeadlines(query);

		//Assert
		_cachingProvider.Verify(c => c.GetOrSetAsync(
			It.IsAny<string>(),
			It.IsAny<Func<Task<GetTopHeadlinesResponse?>>>(),
			CacheName.News), Times.Once);
	}

	[Fact]
	public async Task GetHeadlines_BuildsCacheKeyFromCategoryAndPageSize()
	{
		//Arrange
		var query = new GetTopHeadlinesQuery(NewsCategory.Sports, 15);
		var expectedKey = $"{CacheName.News}_sports_15";

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				expectedKey,
				It.IsAny<Func<Task<GetTopHeadlinesResponse?>>>(),
				CacheName.News))
			.ReturnsAsync((GetTopHeadlinesResponse?)null);

		//Act
		await _sut.GetHeadlines(query);

		//Assert
		_cachingProvider.Verify(c => c.GetOrSetAsync(
			expectedKey,
			It.IsAny<Func<Task<GetTopHeadlinesResponse?>>>(),
			It.IsAny<string>()), Times.Once);
	}

	[Fact]
	public async Task GetHeadlines_ReturnsCachedResult()
	{
		//Arrange
		var query = new GetTopHeadlinesQuery();
		var cached = new GetTopHeadlinesResponse([]);

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				It.IsAny<string>(),
				It.IsAny<Func<Task<GetTopHeadlinesResponse?>>>(),
				It.IsAny<string>()))
			.ReturnsAsync(cached);

		//Act
		var result = await _sut.GetHeadlines(query);

		//Assert
		result.Should().BeSameAs(cached);
		_innerProvider.Verify(p => p.GetHeadlines(query), Times.Never);
	}
}