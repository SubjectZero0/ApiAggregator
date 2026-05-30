using Application.Models.Weather;
using FluentAssertions;
using Gateway.Clients;
using Gateway.Models;
using Gateway.Providers;
using Gateway.Utils.Mappers;
using Moq;

namespace GatewayTests.Providers;

public class WeatherProviderTests
{
	private readonly Mock<IGeocodingClient> _geocodingClient = new();
	private readonly Mock<IWeatherClient> _weatherClient = new();
	private readonly Mock<IMapper<CurrentWeatherData, CurrentWeatherResponse>> _mapper = new();
	private readonly WeatherProvider _sut;

	public WeatherProviderTests()
	{
		_sut = new WeatherProvider(_geocodingClient.Object, _weatherClient.Object, _mapper.Object);
	}

	[Fact]
	public async Task GetWeather_WhenGeocodingReturnsNull_ReturnsNull()
	{
		//Arrange
		var query = new GetCurrentWeatherQuery("Unknown", "XX");
		_geocodingClient
			.Setup(c => c.GetGeocoding(query.CityName, query.CountryCode))
			.ReturnsAsync((Geocoding?)null);

		//Act
		var result = await _sut.GetWeather(query);

		//Assert
		result.Should().BeNull();
		_weatherClient.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task GetWeather_WhenWeatherClientReturnsNull_ReturnsNull()
	{
		//Arrange
		var query = new GetCurrentWeatherQuery("London", "GB");
		var geocoding = new Geocoding { Latitude = 51.5, Longitude = -0.1 };

		_geocodingClient
			.Setup(c => c.GetGeocoding(query.CityName, query.CountryCode))
			.ReturnsAsync(geocoding);

		_weatherClient
			.Setup(c => c.GetCurrentWeather(geocoding.Latitude, geocoding.Longitude))
			.ReturnsAsync((CurrentWeatherData?)null);

		//Act
		var result = await _sut.GetWeather(query);

		//Assert
		result.Should().BeNull();
	}

	[Fact]
	public async Task GetWeather_WhenBothSucceed_ReturnsMappedResponse()
	{
		//Arrange
		var query = new GetCurrentWeatherQuery("London", "GB");
		var geocoding = new Geocoding { Latitude = 51.5, Longitude = -0.1 };

		var weatherData = new CurrentWeatherData
		{
			Weather = [],
			MainWeatherProps = new MainWeatherProps(),
			Wind = new Wind()
		};

		var expected = new CurrentWeatherResponse(
			[],
			new MainWeatherPropsResponse(0, 0, 0, 0, 0, 0, 0, 0),
			new WindResponse(0, 0, 0),
			0);

		_geocodingClient
			.Setup(c => c.GetGeocoding(query.CityName, query.CountryCode))
			.ReturnsAsync(geocoding);

		_weatherClient
			.Setup(c => c.GetCurrentWeather(geocoding.Latitude, geocoding.Longitude))
			.ReturnsAsync(weatherData);

		_mapper
			.Setup(m => m.Map(weatherData))
			.Returns(expected);

		//Act
		var result = await _sut.GetWeather(query);

		//Assert
		result.Should().BeSameAs(expected);
	}
}