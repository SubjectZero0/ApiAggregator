using Application.Services.Dashboard;
using Application.Services.Dashboard.Finance;
using Application.Services.Dashboard.News;
using Application.Services.Dashboard.Weather;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public static partial class ApplicationModule
	{
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<IGetCurrentWeatherService, GetCurrentWeatherService>();
			services.AddScoped<IGetTopHeadlinesService, GetTopHeadlinesService>();
			services.AddScoped<IGetMarketSummaryService, GetMarketSummaryService>();
			services.AddScoped<IDashBoardService, DashBoardService>();

			return services;
		}
	}
}