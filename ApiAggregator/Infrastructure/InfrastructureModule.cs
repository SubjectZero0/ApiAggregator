using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static partial class InfrastructureModule
	{
		public static IServiceCollection AddInfrastructureModule(this IServiceCollection services)
		{
			services
				.AddCachingProviders()
				.AddDecorators();

			return services;
		}
	}
}