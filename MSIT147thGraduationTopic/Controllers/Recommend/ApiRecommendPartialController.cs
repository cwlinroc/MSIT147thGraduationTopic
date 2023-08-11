using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;
using MSIT147thGraduationTopic.Models.Services;

namespace MSIT147thGraduationTopic.Controllers.Recommend
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiRecommendPartialController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly RecommendPartialService _service;

        public ApiRecommendPartialController(GraduationTopicContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _service = new(context, accessor);
        }


        [HttpGet("favorspecs/{merchandiseId}")]
        public async Task<ActionResult<List<SpecDisplyDto>>> GetFavorSpecs(int? merchandiseId)
        {
            return await _service.GetFavorSpecs(merchandiseId);
        }


    }
}
