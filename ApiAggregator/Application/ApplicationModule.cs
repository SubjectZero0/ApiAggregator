using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public static partial class ApplicationModule
	{
		public static IServiceCollection AddApplicationModule(this IServiceCollection services)
		{
			return services
				.AddServices();
		}
	}
}