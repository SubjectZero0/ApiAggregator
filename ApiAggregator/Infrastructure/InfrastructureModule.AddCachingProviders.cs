using Application.Caching;
using Infrastructure.Caching.HybridCaching;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static partial class InfrastructureModule
	{
		public static IServiceCollection AddCachingProviders(this IServiceCollection services)
		{
			services.AddHybridCache(options =>
			{
				options.MaximumPayloadBytes = 10_485_760;
			});

			services.AddSingleton<IHybridCacheOptionsFactory, HybridCacheOptionsFactory>();
			services.AddSingleton<ICachingProvider, HybridCacheProvider>();

			return services;
		}
	}
}