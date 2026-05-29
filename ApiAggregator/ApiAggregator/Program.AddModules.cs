using Application;
using Gateway;
using Infrastructure;

namespace ApiAggregator
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddModules(this WebApplicationBuilder builder)
		{
			builder.Services.AddGatewayModule(builder.Configuration);
			builder.Services.AddApplicationModule(builder.Configuration);
			builder.Services.AddInfrastructureModule();

			return builder;
		}
	}
}