using Gateway;

namespace ApiAggregator
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddModules(this WebApplicationBuilder builder)
		{
			builder.Services.AddGatewayModule(builder.Configuration);

			return builder;
		}
	}
}