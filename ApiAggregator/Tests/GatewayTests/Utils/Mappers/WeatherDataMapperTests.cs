using FluentAssertions;
using Gateway.Models;
using Gateway.Utils.Mappers;

namespace GatewayTests.Utils.Mappers;

public class WeatherDataMapperTests
{
	private readonly WeatherDataMapper _sut = new();

	[Fact]
	public void Map_MapsAllFields()
	{
		//Arrange
		var input = new CurrentWeatherData
		{
			Weather = [new Weather { Main = "Clear", Description = "clear sky" }],
			MainWeatherProps = new MainWeatherProps
			{
				Temp = 20.5f,
				FeelsLike = 19.0f,
				TempMin = 18.0f,
				TempMax = 22.0f,
				Pressure = 1013,
				Humidity = 65,
				SeaLevel = 1013,
				GroundLevel = 1010
			},
			Wind = new Wind { Speed = 5.5f, Deg = 270, Gust = 8.0f },
			Visibility = 10000
		};

		//Act
		var result = _sut.Map(input);

		//Assert
		result.Weather.Should().HaveCount(1);
		result.Weather.First().Main.Should().Be("Clear");
		result.Weather.First().Description.Should().Be("clear sky");

		result.MainWeatherProps.Temp.Should().Be(20.5f);
		result.MainWeatherProps.FeelsLike.Should().Be(19.0f);
		result.MainWeatherProps.TempMin.Should().Be(18.0f);
		result.MainWeatherProps.TempMax.Should().Be(22.0f);
		result.MainWeatherProps.Pressure.Should().Be(1013);
		result.MainWeatherProps.Humidity.Should().Be(65);
		result.MainWeatherProps.SeaLevel.Should().Be(1013);
		result.MainWeatherProps.GroundLevel.Should().Be(1010);

		result.Wind.Speed.Should().Be(5.5f);
		result.Wind.Deg.Should().Be(270);
		result.Wind.Gust.Should().Be(8.0f);

		result.Visibility.Should().Be(10000);
	}

	[Fact]
	public void Map_MultipleWeatherEntries_MapsAll()
	{
		//Arrange
		var input = new CurrentWeatherData
		{
			Weather =
			[
				new Weather { Main = "Rain", Description = "light rain" },
				new Weather { Main = "Clouds", Description = "broken clouds" }
			],
			MainWeatherProps = new MainWeatherProps(),
			Wind = new Wind()
		};

		//Act
		var result = _sut.Map(input);

		//Assert
		result.Weather.Should().HaveCount(2);
	}
}