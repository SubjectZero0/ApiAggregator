namespace Application.Models.Weather
{
	public class GetCurrentWeatherQuery
	{
		public string CityName { get; }

		public string CountryCode { get; }

		public GetCurrentWeatherQuery()
		{
			CityName = string.Empty;
			CountryCode = string.Empty;
		}

		public GetCurrentWeatherQuery(string cityName, string countryCode)
		{
			CityName = cityName;
			CountryCode = countryCode;
		}
	}

	public class CurrentWeatherResponse
	{
		public IReadOnlyCollection<WeatherResponse> Weather { get; }

		public MainWeatherPropsResponse MainWeatherProps { get; }

		public WindResponse Wind { get; }

		public int Visibility { get; }

		public CurrentWeatherResponse(IReadOnlyCollection<WeatherResponse> weather, MainWeatherPropsResponse mainWeatherProps, WindResponse wind, int visibility)
		{
			Weather = weather;
			MainWeatherProps = mainWeatherProps;
			Wind = wind;
			Visibility = visibility;
		}
	}

	public class WindResponse
	{
		public float Speed { get; }
		public int Deg { get; }
		public float Gust { get; }

		public WindResponse(float speed, int deg, float gust)
		{
			Speed = speed;
			Deg = deg;
			Gust = gust;
		}
	}

	public class WeatherResponse
	{
		public string Main { get; }
		public string Description { get; }

		public WeatherResponse(string main, string description)
		{
			Main = main;
			Description = description;
		}
	}

	public class MainWeatherPropsResponse
	{
		public float Temp { get; }

		public float FeelsLike { get; }

		public float TempMin { get; }

		public float TempMax { get; }

		public int Pressure { get; }

		public int Humidity { get; }

		public int SeaLevel { get; }

		public int GroundLevel { get; }

		public MainWeatherPropsResponse(
			float temp,
			float feelsLike,
			float tempMin,
			float tempMax,
			int pressure,
			int humidity,
			int seaLevel,
			int groundLevel)
		{
			Temp = temp;
			FeelsLike = feelsLike;
			TempMin = tempMin;
			TempMax = tempMax;
			Pressure = pressure;
			Humidity = humidity;
			SeaLevel = seaLevel;
			GroundLevel = groundLevel;
		}
	}
}