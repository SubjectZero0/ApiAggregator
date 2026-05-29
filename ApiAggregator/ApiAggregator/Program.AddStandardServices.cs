namespace ApiAggregator
{
	public static partial class Program
	{
		public static WebApplicationBuilder AddStandardServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
			builder.Services.AddProblemDetails();

			return builder;
		}
	}
}