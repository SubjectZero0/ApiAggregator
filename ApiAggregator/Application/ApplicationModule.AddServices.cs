using Application.Services.Dashboard.Weather;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public static partial class ApplicationModule
	{
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<IGetCurrentWeatherService, GetCurrentWeatherService>();

			return services;
		}
	}
}