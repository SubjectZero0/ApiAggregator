using Application.Services.Metrics;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public static partial class GatewayModule
	{
		public static IServiceCollection AddMetrics(this IServiceCollection services)
		{
			services.AddTransient<OutgoingApiCallMetricsHandler>();
			services.AddScoped<IMetricsService, MetricsService>();

			return services;
		}
	}
}