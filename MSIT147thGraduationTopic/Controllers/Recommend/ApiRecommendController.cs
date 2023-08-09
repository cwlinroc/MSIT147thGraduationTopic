using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;

namespace MSIT147thGraduationTopic.Controllers.Recommend
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiRecommendController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly RecommendService _service;

        public ApiRecommendController(GraduationTopicContext context)
        {
            _context = context;
            _service = new(context);
        }


        public record RateDataRecord(int num, string data);
        [HttpPut("ratedata")]
        public async Task<ActionResult<int>> UpdateRateEvaluationFunc(RateDataRecord record)
        {
            string col = record.data.ToLower() switch
            {
                "evaluationweight" => "[EvaluationWeight]",
                "purchasedeight" => "[PurchasedWeight]",
                "manuallyweight" => "[ManuallyWeight]",
                "rateevaluationfunc" => "[RateEvaluationFunc]",
                "ratepurchasefunc" => "[RatePurchaseFunc]",
                _ => "",
            };
            if (string.IsNullOrEmpty(col) || record.num <= 0) return -1;
            return await _service.UpdateRatingData(record.num, col);
        }



        [HttpGet("mostpopularspecs")]
        public async Task<ActionResult<List<string>>> GetMostPopularSpecsName(int top = 10)
        {
            return await _service.GetMostPopularSpecsName(top);
        }


        //public async Task<IActionResult> CalculatePopularity()
        //{

        //}

    }
}
