using Application.Services.Metrics;
using Gateway.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway
{
	public static partial class GatewayModule
	{
		public static IServiceCollection AddClients(this IServiceCollection services)
		{
			services
				.AddHttpClient<IGeocodingClient, GeocodingClient>()
				.AddHttpMessageHandler<OutgoingApiCallMetricsHandler>();

			services
				.AddHttpClient<IWeatherClient, WeatherClient>()
				.AddHttpMessageHandler<OutgoingApiCallMetricsHandler>();

			services
				.AddHttpClient<IMarketClient, MarketClient>()
				.AddHttpMessageHandler<OutgoingApiCallMetricsHandler>();

			services
				.AddHttpClient<INewsClient, NewsClient>()
				.AddHttpMessageHandler<OutgoingApiCallMetricsHandler>();

			return services;
		}
	}
}