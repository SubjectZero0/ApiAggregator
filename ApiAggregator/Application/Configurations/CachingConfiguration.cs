using System.ComponentModel.DataAnnotations;

namespace Application.Configurations
{
	public class CachingConfiguration
	{
		[Required]
		public TimeSpan MarketExpiry { get; set; } = TimeSpan.FromHours(2);

		[Required]
		public TimeSpan WeatherExpiry { get; set; } = TimeSpan.FromHours(8);

		[Required]
		public TimeSpan NewsExpiry { get; set; } = TimeSpan.FromHours(1);
	}
}