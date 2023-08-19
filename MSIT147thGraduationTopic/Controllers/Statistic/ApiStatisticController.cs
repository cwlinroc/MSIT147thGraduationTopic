using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Statistic;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.Services;
using System.Data;

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
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<ActionResult<SaleChartDto?>> GetSaleChart(string measurement, string classification, int daysBefore)
        {
            return await _service.GetSaleChart(measurement, classification, daysBefore);
        }

        [HttpGet("MostSalesMerchandises")]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetMostSalesMerchandises(string classification, int daysBefore)
        {
            return (await _service.GetMostSalesMerchandises(classification, daysBefore))
                .Select(o => new { o.label, o.data }).ToList();
        }


        [HttpGet("saletrend")]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<ActionResult<SaleTrendDto?>> GetSalesTrend(
            string measurement,
            string classification,
            string timeUnit,
            int timeIntervals)
        {
            return await _service.GetSalesTrend(measurement, classification, timeUnit, timeIntervals);
        }


        [HttpGet("evaluationscores/{id}")]
        public async Task<ActionResult<int[]?>> GetEvaluationScores(int id)
        {
            return await _service.GetEvaluationScores(id);
        }


        [HttpGet("merchandisetrend")]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<ActionResult<TimeTrendDto?>> GetMerchandiseTrend(
            string measurement,
            string classification,
            string timeUnit,
            int id)
        {
            return await _service.GetMerchandiseTrend(measurement, classification, timeUnit, id);
        }

        [HttpGet("GetAutoCompleteNames")]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<ActionResult<IEnumerable<string>?>> GetAutoCompleteNames(string queryCol, string keyword)
        {
            return await _service.GetAutoCompleteNames(queryCol, keyword);
        }

        [HttpGet("GetSearchedId")]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<ActionResult<dynamic>> GetSearchedId(string queryCol, string keyword)
        {
            (int id, string name) = await _service.GetSearchedId(queryCol, keyword);
            return new { id, name };
        }

        [HttpGet("GetMerchandiseRadar")]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<ActionResult<MerchandiseRadarDto?>> GetMerchandiseRadar(string measurement, int id)
        {
            return await _service.GetMerchandiseRadar(measurement, id);
        }



    }
}
