using Application.Models.Weather;
using Application.Providers;
using Application.Services.Dashboard.Weather;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationTests.Services.Dashboard.Weather;

public class GetCurrentWeatherServiceTests
{
	private readonly Mock<IWeatherProvider> _weatherProvider = new();
	private readonly Mock<ILogger<GetCurrentWeatherService>> _logger = new();
	private readonly IGetCurrentWeatherService _sut;

	public GetCurrentWeatherServiceTests()
	{
		_sut = new GetCurrentWeatherService(_weatherProvider.Object, _logger.Object);
	}

	[Fact]
	public async Task Get_WhenProviderReturnsResponse_ReturnsIt()
	{
		//Arrange
		var query = new GetCurrentWeatherQuery("London", "GB");

		var expected = new CurrentWeatherResponse(
			[],
			new MainWeatherPropsResponse(20, 19, 18, 22, 1013, 65, 1013, 1010),
			new WindResponse(5.5f, 270, 8.0f),
			10000);

		_weatherProvider
			.Setup(p => p.GetWeather(query))
			.ReturnsAsync(expected);

		//Act
		var result = await _sut.Get(query);

		//Assert
		result.Should().BeSameAs(expected);
	}

	[Fact]
	public async Task Get_WhenProviderReturnsNull_ReturnsNull()
	{
		//Arrange
		var query = new GetCurrentWeatherQuery("Unknown", "XX");

		_weatherProvider
			.Setup(p => p.GetWeather(query))
			.ReturnsAsync((CurrentWeatherResponse?)null);

		//Act
		var result = await _sut.Get(query);

		//Assert
		result.Should().BeNull();
	}
}