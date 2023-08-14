using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.Services;

namespace MSIT147thGraduationTopic.Controllers.Recommend
{
    public class RecommendBackstageController : Controller
    {

        private readonly GraduationTopicContext _context;
        private readonly RecommendService _service;

        public RecommendBackstageController(GraduationTopicContext context)
        {
            _context = context;
            _service = new(context);
        }


        [Authorize(Roles = "管理員,經理")]
        public async Task<IActionResult> Index()
        {
            var ratingData = await _service.GetRatingData();
            ViewBag.RatingData = ratingData;

            return View();
        }
    }
}
