using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models;
using MSIT147thGraduationTopic.Models.Services;
using System.Diagnostics;

namespace MSIT147thGraduationTopic.Controllers.Home
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly GraduationTopicContext _context;
        private readonly HomeServices _service;
        public HomeController(GraduationTopicContext context)
        {
            _context = context;
            _service = new HomeServices(context);
        }

        public IActionResult Index()
        {
            var popularSpecs = _service.GetMostPopularSpecs(8);
            var favorableSpecs = _service.GetMostFavorableSpecs(8);
            var newSpecs = _service.GetNewestSpecs(8);

            return View((popularSpecs, favorableSpecs, newSpecs));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}