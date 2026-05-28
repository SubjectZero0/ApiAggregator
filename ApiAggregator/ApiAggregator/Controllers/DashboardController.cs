using Application.Models.Weather;
using Application.Services.Dashboard.Weather;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggregator.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DashboardController : ControllerBase
	{
		private readonly IGetCurrentWeatherService _weatherService;

		public DashboardController(IGetCurrentWeatherService weatherService)
		{
			_weatherService = weatherService;
		}

		[HttpGet]
		[Route("/geocoding")]
		public async Task<ActionResult> GetGeocoding([FromQuery] GetCurrentWeatherQuery query)
		{
			var result = await _weatherService.Get(query);

			return Ok(result);
		}
	}
}