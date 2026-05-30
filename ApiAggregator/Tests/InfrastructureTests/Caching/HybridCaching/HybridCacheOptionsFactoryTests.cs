using Application.Configurations;
using FluentAssertions;
using Infrastructure.Caching.HybridCaching;
using Microsoft.Extensions.Options;
using static Common.Constants;

namespace InfrastructureTests.Caching.HybridCaching;

public class HybridCacheOptionsFactoryTests
{
	private static HybridCacheOptionsFactory BuildSut(
		TimeSpan? marketExpiry = null,
		TimeSpan? weatherExpiry = null,
		TimeSpan? newsExpiry = null)
	{
		var config = new CachingConfiguration
		{
			MarketExpiry = marketExpiry ?? TimeSpan.FromHours(2),
			WeatherExpiry = weatherExpiry ?? TimeSpan.FromHours(8),
			NewsExpiry = newsExpiry ?? TimeSpan.FromHours(1)
		};
		return new HybridCacheOptionsFactory(Options.Create(config));
	}

	[Fact]
	public void GetOptions_ForDailyMarkets_ReturnsExpiryUntilMidnight()
	{
		//Arrange
		var sut = BuildSut();

		//Act
		var options = sut.GetOptions(CacheName.DailyMarkets);

		//Assert
		options.Expiration.Should().NotBeNull();
		options.Expiration!.Value.Should().BeGreaterThan(TimeSpan.Zero);
		options.Expiration.Value.Should().BeLessThanOrEqualTo(TimeSpan.FromHours(24));
	}

	[Fact]
	public void GetOptions_ForNews_ReturnsConfiguredNewsExpiry()
	{
		//Arrange
		var newsExpiry = TimeSpan.FromMinutes(30);
		var sut = BuildSut(newsExpiry: newsExpiry);

		//Act
		var options = sut.GetOptions(CacheName.News);

		//Assert
		options.Expiration.Should().Be(newsExpiry);
	}

	[Fact]
	public void GetOptions_ForWeather_ReturnsConfiguredWeatherExpiry()
	{
		//Arrange
		var weatherExpiry = TimeSpan.FromHours(4);
		var sut = BuildSut(weatherExpiry: weatherExpiry);

		//Act
		var options = sut.GetOptions(CacheName.Weather);

		//Assert
		options.Expiration.Should().Be(weatherExpiry);
	}

	[Fact]
	public void GetOptions_ForFinance_ReturnsConfiguredMarketExpiry()
	{
		//Arrange
		var marketExpiry = TimeSpan.FromHours(3);
		var sut = BuildSut(marketExpiry: marketExpiry);

		//Act
		var options = sut.GetOptions(CacheName.Finance);

		//Assert
		options.Expiration.Should().Be(marketExpiry);
	}

	[Fact]
	public void GetOptions_ForUnknownCacheName_ThrowsArgumentException()
	{
		//Arrange
		var sut = BuildSut();

		//Act
		var act = () => sut.GetOptions("NonExistentCache");

		//Assert
		act.Should().Throw<ArgumentException>();
	}
}