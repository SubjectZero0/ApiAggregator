using System.Text.Json.Serialization;

namespace Gateway.Models
{
	public class CurrentWeatherData
	{
		[JsonPropertyName("coord")]
		public Coord Coordinates { get; set; } = new Coord();

		public IReadOnlyCollection<Weather> Weather { get; set; } = [];

		[JsonPropertyName("base")]
		public string InstrumentBase { get; set; } = string.Empty;

		[JsonPropertyName("main")]
		public MainWeatherProps MainWeatherProps { get; set; } = new MainWeatherProps();

		public int Visibility { get; set; }

		public Wind Wind { get; set; } = new Wind();

		public Dictionary<string, float>? Rain { get; set; }

		public Dictionary<string, int>? Clouds { get; set; }

		public long Dt { get; set; }

		public Sys Sys { get; set; } = new Sys();

		public int Timezone { get; set; }

		public long Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public int Cod { get; set; }
	}

	public class Coord
	{
		[JsonPropertyName("lat")]
		public double Latitude { get; set; }

		[JsonPropertyName("lon")]
		public double Longitude { get; set; }
	}

	public class MainWeatherProps
	{
		public float Temp { get; set; }

		[JsonPropertyName("feels_like")]
		public float FeelsLike { get; set; }

		[JsonPropertyName("temp_min")]
		public float TempMin { get; set; }

		[JsonPropertyName("temp_max")]
		public float TempMax { get; set; }

		public int Pressure { get; set; }

		public int Humidity { get; set; }

		[JsonPropertyName("sea_level")]
		public int SeaLevel { get; set; }

		[JsonPropertyName("grnd_level")]
		public int GroundLevel { get; set; }
	}

	public class Wind
	{
		public float Speed { get; set; }
		public int Deg { get; set; }
		public float Gust { get; set; }
	}

	public class Sys
	{
		public int Type { get; set; }
		public int Id { get; set; }
		public string Country { get; set; } = string.Empty;
		public long Sunrise { get; set; }
		public long Sunset { get; set; }
	}

	public class Weather
	{
		public int Id { get; set; }
		public string Main { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Icon { get; set; } = string.Empty;
	}
}