using System.Text.Json.Serialization;

namespace Gateway.Models
{
	public class Geocoding
	{
		[JsonPropertyName("zip")]
		public string ZipCode { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("local_names")]
		public Dictionary<string, string>? LocalNames { get; set; }

		public string Country { get; set; } = string.Empty;

		public string State { get; set; } = string.Empty;

		[JsonPropertyName("lat")]
		public double Latitude { get; set; }

		[JsonPropertyName("lon")]
		public double Longitude { get; set; }
	}
}