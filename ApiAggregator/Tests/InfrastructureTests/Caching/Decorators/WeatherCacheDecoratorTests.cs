using Application.Caching;
using Application.Models.News;
using Application.Models.Weather;
using Application.Providers;
using FluentAssertions;
using Infrastructure.Caching.Decorators;
using Moq;
using static Common.Constants;

namespace InfrastructureTests.Caching.Decorators;

public class WeatherCacheDecoratorTests
{
	private readonly Mock<IWeatherProvider> _innerProvider = new();
	private readonly Mock<ICachingProvider> _cachingProvider = new();
	private readonly WeatherCacheDecorator _sut;

	public WeatherCacheDecoratorTests()
	{
		_sut = new WeatherCacheDecorator(_innerProvider.Object, _cachingProvider.Object);
	}

	[Fact]
	public async Task GetWeather_CallsCachingProviderWithWeatherCacheName()
	{
		//Arrange
		var query = new GetCurrentWeatherQuery("London", "GB");

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				It.IsAny<string>(),
				It.IsAny<Func<Task<CurrentWeatherResponse?>>>(),
				CacheName.Weather))
			.ReturnsAsync((CurrentWeatherResponse?)null);

		//Act
		await _sut.GetWeather(query);

		//Assert
		_cachingProvider.Verify(c => c.GetOrSetAsync(
			It.IsAny<string>(),
			It.IsAny<Func<Task<CurrentWeatherResponse?>>>(),
			CacheName.Weather), Times.Once);
	}

	[Fact]
	public async Task GetWeather_NormalizesCityNameAndCountryCodeInCacheKey()
	{
		//Arrange
		var query = new GetCurrentWeatherQuery("  LONDON  ", "  GB  ");
		var expectedKey = $"{CacheName.Weather}_london_gb";

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				expectedKey,
				It.IsAny<Func<Task<CurrentWeatherResponse?>>>(),
				CacheName.Weather))
			.ReturnsAsync((CurrentWeatherResponse?)null);

		//Act
		await _sut.GetWeather(query);

		//Assert
		_cachingProvider.Verify(c => c.GetOrSetAsync(
			expectedKey,
			It.IsAny<Func<Task<CurrentWeatherResponse?>>>(),
			It.IsAny<string>()), Times.Once);
	}

	[Fact]
	public async Task GetWeather_ReturnsCachedResult()
	{
		//Arrange
		var query = new GetCurrentWeatherQuery("Paris", "FR");
		var cached = new CurrentWeatherResponse(
			[],
			new MainWeatherPropsResponse(0, 0, 0, 0, 0, 0, 0, 0),
			new WindResponse(0, 0, 0),
			0);

		_cachingProvider
			.Setup(c => c.GetOrSetAsync(
				It.IsAny<string>(),
				It.IsAny<Func<Task<CurrentWeatherResponse?>>>(),
				It.IsAny<string>()))
			.ReturnsAsync(cached);

		//Act
		var result = await _sut.GetWeather(query);

		//Assert
		result.Should().BeSameAs(cached);
		_innerProvider.Verify(p => p.GetWeather(It.IsAny<GetCurrentWeatherQuery>()), Times.Never);
	}
}