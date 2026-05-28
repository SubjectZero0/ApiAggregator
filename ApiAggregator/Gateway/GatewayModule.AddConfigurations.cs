using ApiAggregator.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway
{
	public static partial class GatewayModule
	{
		public static IServiceCollection AddGatewayConfigurations(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddOptionsWithValidateOnStart<NewsApiConfiguration>()
				.Bind(configuration.GetSection("NewsApiConfiguration"))
				.ValidateDataAnnotations();

			services
				.AddOptionsWithValidateOnStart<WeatherApiConfiguration>()
				.Bind(configuration.GetSection("WeatherApiConfiguration"))
				.ValidateDataAnnotations();

			services
				.AddOptionsWithValidateOnStart<GeocodingApiConfiguration>()
				.Bind(configuration.GetSection("GeocodingApiConfiguration"))
				.ValidateDataAnnotations();

			services
				.AddOptionsWithValidateOnStart<MassiveApiConfiguration>()
				.Bind(configuration.GetSection("MassiveApiConfiguration"))
				.ValidateDataAnnotations();

			return services;
		}
	}
}