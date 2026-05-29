using Application.Providers;
using Gateway.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway
{
	public static partial class GatewayModule
	{
		public static IServiceCollection AddProviders(this IServiceCollection services)
		{
			services.AddScoped<IWeatherProvider, WeatherProvider>();
			services.AddScoped<INewsProvider, NewsProvider>();
			services.AddScoped<IMarketProvider, MarketProvider>();

			return services;
		}
	}
}