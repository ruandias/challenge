using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.Contracts;

namespace TaskManagement.API.Controllers
{
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("average-tasks-completed")]
        public async Task<IActionResult> GetAverageTasksCompleted([FromQuery] string role)
        {
            var report = await _reportService.GetAverageTasksCompletedAsync(role);
            return Ok(report);
        }
    }
}
