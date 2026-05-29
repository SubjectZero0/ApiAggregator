using Application.Providers;
using Infrastructure.Caching.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static partial class InfrastructureModule
	{
		public static IServiceCollection AddDecorators(this IServiceCollection services)
		{
			services.Decorate<IMarketProvider, MarketCacheDecorator>();
			services.Decorate<INewsProvider, NewsCacheDecorator>();
			services.Decorate<IWeatherProvider, WeatherCacheDecorator>();

			return services;
		}
	}
}