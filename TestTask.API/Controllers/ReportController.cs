using Microsoft.AspNetCore.Mvc;
using TestTask.Services;
using TestTask.Utils.DTOs;

namespace TestTask.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController: ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<List<YearlyReportDTO>>> GetReport()
        {
            return await _reportService.GetPopularityReport();
        }
    }
}
