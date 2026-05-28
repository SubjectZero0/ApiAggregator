using Application.Models.Weather;
using Application.Utils.Mappers;
using Gateway.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public static partial class ApplicationModule
	{
		public static IServiceCollection AddUtils(this IServiceCollection services)
		{
			services.AddSingleton<IMapper<CurrentWeatherData, CurrentWeatherResponse>, GetCurrentWeatherDataMapper>();

			return services;
		}
	}
}