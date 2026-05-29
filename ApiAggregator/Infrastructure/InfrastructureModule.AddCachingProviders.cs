using Application.Caching;
using Infrastructure.Caching.MemoryCache;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static partial class InfrastructureModule
	{
		public static IServiceCollection AddCachingProviders(this IServiceCollection services)
		{
			services.AddMemoryCache();
			services.AddSingleton<ICachingProvider, MemoryCachingProvider>();

			return services;
		}
	}
}