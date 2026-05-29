using Application.Models.Finance;
using Application.Models.News;
using Application.Models.Weather;
using Gateway.Models;
using Gateway.Utils;
using Gateway.Utils.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway
{
	public static partial class GatewayModule
	{
		public static IServiceCollection AddUtils(this IServiceCollection services)
		{
			services.AddSingleton<IMapper<CurrentWeatherData, CurrentWeatherResponse>, WeatherDataMapper>();
			services.AddSingleton<IMapper<Article[], ArticleResponse[]>, ArticleMapper>();
			services.AddSingleton<IMapper<MassiveStockTicker[], MarketResponse[]>, MarketMapper>();
			services.AddSingleton<IMarketSorter, MarketSorter>();

			return services;
		}
	}
}