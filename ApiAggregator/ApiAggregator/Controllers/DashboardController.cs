using Application.Models;
using Application.Services.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregator.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DashboardController : ControllerBase
	{
		private readonly IDashBoardService _dashboardService;

		public DashboardController(IDashBoardService dashboardService)
		{
			_dashboardService = dashboardService;
		}

		[HttpGet]
		[Route("/dashboard")]
		public async Task<ActionResult> GetDashboard([FromQuery] GetDashBoardQuery query)
		{
			var result = await _dashboardService.GetDashBoard(query);

			return Ok(result);
		}
	}
}