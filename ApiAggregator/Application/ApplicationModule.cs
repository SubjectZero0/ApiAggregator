using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public static partial class ApplicationModule
	{
		public static IServiceCollection AddApplicationModule(this IServiceCollection services, IConfiguration configuration)
		{
			return services
				.AddConfigurations(configuration)
				.AddMetrics()
				.AddServices();
		}
	}
}