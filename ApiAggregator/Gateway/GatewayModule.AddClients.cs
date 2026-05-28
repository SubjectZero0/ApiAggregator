using Gateway.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway
{
	public static partial class GatewayModule
	{
		public static IServiceCollection AddClients(this IServiceCollection services)
		{
			services.AddHttpClient<IGeocodingClient, GeocodingCLient>();
			services.AddHttpClient<IWeatherClient, WeatherClient>();
			services.AddHttpClient<IMarketClient, MarketClient>();
			services.AddHttpClient<INewsClient, NewsClient>();

			return services;
		}
	}
}