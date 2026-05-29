using Application.Caching;
using Infrastructure.Caching.HybridCacheProvider;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static partial class InfrastructureModule
	{
		public static IServiceCollection AddCachingProviders(this IServiceCollection services)
		{
			services.AddHybridCache();
			services.AddSingleton<ICachingProvider, HybridCacheProvider>();

			return services;
		}
	}
}