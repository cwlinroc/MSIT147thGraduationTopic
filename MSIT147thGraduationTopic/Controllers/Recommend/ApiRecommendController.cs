using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers.Recommend
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiRecommendController : ControllerBase
    {

        private readonly GraduationTopicContext _context;

        public ApiRecommendController(GraduationTopicContext context)
        {
            _context = context;
        }
        




        //public async Task<IActionResult> CalculatePopularity()
        //{

        //}

    }
}
