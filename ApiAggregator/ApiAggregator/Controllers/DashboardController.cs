using Application.Models;
using Application.Services.Dashboard;
using Application.Services.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregator.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DashboardController : ControllerBase
	{
		private readonly IDashBoardService _dashboardService;
		private readonly IMetricsService _metricsService;

		public DashboardController(IDashBoardService dashboardService, IMetricsService metricsService)
		{
			_dashboardService = dashboardService;
			_metricsService = metricsService;
		}

		[HttpGet]
		[Route("/dashboard")]
		public async Task<ActionResult> GetDashboard([FromQuery] GetDashBoardQuery query)
		{
			var result = await _dashboardService.GetDashBoard(query);

			return Ok(result);
		}

		[HttpGet]
		[Route("/metrics")]
		public ActionResult GetMetrics()
		{
			var metrics = _metricsService.GetOutgoingApiCallMetrics();

			return Ok(metrics);
		}
	}
}