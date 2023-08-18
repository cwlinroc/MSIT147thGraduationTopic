using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Drawing;

namespace MSIT147thGraduationTopic.Controllers
{
    public class MallController : Controller
    {
        private readonly GraduationTopicContext _context;
        public MallController(GraduationTopicContext context)
        {
            _context = context;
        }

        public IActionResult Index(string txtKeyword = "", int searchCondition = 1, int displayorder = 0,
                                    int pageSize = 20, int PageIndex = 1, int sideCategoryId = 0,
                                    int? minPrice = null, int? maxPrice = null, int tagId = 0)
        {
            ViewBag.txtKeyword = txtKeyword;
            ViewBag.searchCondition = searchCondition;
            ViewBag.displayorder = displayorder;
            ViewBag.pageSize = pageSize;
            ViewBag.PageIndex = PageIndex;
            ViewBag.sideCategoryId = sideCategoryId;
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            ViewBag.tagId = tagId;

            return View();
        }

        public IActionResult Viewpage(int MerchandiseId, int SpecId)
        {
            //紀錄最近三筆瀏覽商品
            int? last_2 = HttpContext.Session.GetInt32("Last_2");
            if (last_2.HasValue)
                HttpContext.Session.SetInt32("Last_3", last_2.Value);
            int? last_1 = HttpContext.Session.GetInt32("Last_1");
            if (last_1.HasValue)
                HttpContext.Session.SetInt32("Last_2", last_1.Value);
            HttpContext.Session.SetInt32("Last_1", MerchandiseId);

            ViewBag.SpecId = SpecId;

            IEnumerable<MallViewPageVM> datas = _context.Specs
                .Where(s => s.MerchandiseId == MerchandiseId).Where(s => s.OnShelf == true).Where(s => s.Amount > 0)
                .Select(o => new MallViewPageVM { spec = o, Score = (o.Evaluations.Any()) ? o.Evaluations.Average(x => x.Score) : 0 }).ToList();

            return View(datas);
        }
    }
}
