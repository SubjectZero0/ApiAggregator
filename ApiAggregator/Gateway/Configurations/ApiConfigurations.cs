using System.ComponentModel.DataAnnotations;

namespace ApiAggregator.Configurations
{
	public class NewsApiConfiguration
	{
		[Required]
		public string ApiKey { get; set; } = string.Empty;

		[Required]
		public string BaseUrl { get; set; } = string.Empty;
	}

	public class WeatherApiConfiguration
	{
		[Required]
		public string ApiKey { get; set; } = string.Empty;

		[Required]
		public string BaseUrl { get; set; } = string.Empty;
	}

	public class GeocodingApiConfiguration
	{
		[Required]
		public string ApiKey { get; set; } = string.Empty;

		[Required]
		public string BaseUrl { get; set; } = string.Empty;
	}

	public class MassiveApiConfiguration
	{
		[Required]
		public string ApiKey { get; set; } = string.Empty;

		[Required]
		public string BaseUrl { get; set; } = string.Empty;
	}
}