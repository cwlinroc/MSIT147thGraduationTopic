using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Statistic;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.Services;

namespace MSIT147thGraduationTopic.Controllers.Statistic
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiStatisticController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly StatisticService _service;

        public ApiStatisticController(GraduationTopicContext context)
        {
            _context = context;
            _service = new StatisticService(context);
        }

        [HttpGet("salechart")]
        public async Task<ActionResult<SaleChartDto?>> GetSaleChart(string measurement, string classification, int monthsBefore)
        {
            return await _service.GetSaleChart(measurement, classification, monthsBefore);
        }





    }
}
