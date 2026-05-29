using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiAggregator
{
	public class GlobalExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<GlobalExceptionHandler> _logger;
		private readonly IProblemDetailsService _problemDetailsService;

		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService)
		{
			_logger = logger;
			_problemDetailsService = problemDetailsService;
		}

		public async ValueTask<bool> TryHandleAsync(
			HttpContext httpContext,
			Exception exception,
			CancellationToken cancellationToken)
		{
			_logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

			var statusCode = HttpStatusCode.InternalServerError;
			var title = "An unexpected error occurred";

			return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
			{
				HttpContext = httpContext,
				Exception = exception,
				ProblemDetails = new ProblemDetails
				{
					Status = (int)statusCode,
					Title = title,
					Detail = exception.Message,
					Instance = httpContext.Request.Path
				}
			});
		}
	}
}