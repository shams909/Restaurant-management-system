using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.Interfaces;
using System.Threading.Tasks;

namespace RMS.Api.Controllers
{
    [Authorize]
    [Authorize(Roles = "Manager")] // Waiters and Chefs cannot see the financial data!
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportingService _reportingService;

        public ReportsController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var data = await _reportingService.GetDailyDashboardAsync();
            return Ok(data);
        }
    }
}
