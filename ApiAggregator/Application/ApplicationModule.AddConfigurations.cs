using Application.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public static partial class ApplicationModule
	{
		public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddOptionsWithValidateOnStart<CachingConfiguration>()
				.Bind(configuration.GetSection("CachingConfiguration"))
				.ValidateDataAnnotations();

			return services;
		}
	}
}