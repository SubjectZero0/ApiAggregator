using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway
{
	public static partial class GatewayModule
	{
		public static IServiceCollection AddGatewayModule(this IServiceCollection services, IConfiguration configuration)
		{
			return services
				.AddGatewayConfigurations(configuration)
				.AddClients();
		}
	}
}