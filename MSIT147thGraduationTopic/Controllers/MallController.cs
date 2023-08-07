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
                                    int? minPrice = null, int? maxPrice = null)// todo 寵物類別TAG查詢
        {
            ViewBag.txtKeyword = txtKeyword;
            ViewBag.searchCondition = searchCondition;
            ViewBag.displayorder = displayorder;
            ViewBag.pageSize = pageSize;
            ViewBag.PageIndex = PageIndex;
            ViewBag.sideCategoryId = sideCategoryId;
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            //todo 用ViewBag傳參數，再用AJAX(ApiMall/DisplaySearchResult/......)

            IEnumerable<MallDisplay> datas = _context.MallDisplays
                .Where(md => md.Display == true).Where(md => md.OnShelf == true).Where(md => md.Amount > 0);

            if (!string.IsNullOrEmpty(txtKeyword))
            {
                datas = searchCondition switch
                {
                    1 => datas.Where(md => md.FullName.Contains(txtKeyword)),
                    2 => datas.Where(md => md.BrandName.Contains(txtKeyword)),
                    3 => datas.Where(md => md.CategoryName.Contains(txtKeyword)),
                    _ => datas
                };
            }

            datas = (sideCategoryId == 0) ? datas : datas.Where(md => md.CategoryId == sideCategoryId);
            datas = (minPrice.HasValue) ? datas.Where(sp => sp.Price >= minPrice) : datas;
            datas = (maxPrice.HasValue) ? datas.Where(sp => sp.Price <= maxPrice) : datas;

            datas = displayorder switch
            {
                0 => datas.OrderByDescending(md => md.SpecId),      //最新商品
                1 => datas.OrderBy(md => md.SpecId),                //由舊到新熱門商品
                2 => datas.OrderByDescending(md => md.Popularity),  //熱門商品
                3 => datas.OrderBy(md => md.Price),                 //價格由低至高
                4 => datas.OrderByDescending(md => md.Price),       //價格由高至低
                _ => datas.OrderByDescending(md => md.SpecId)
            };

            datas = datas.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();
            
            return View(datas);
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

            IEnumerable<Spec> datas = _context.Specs
                .Where(s => s.MerchandiseId == MerchandiseId).Where(s => s.OnShelf == true).Where(s => s.Amount > 0);

            return View(datas);
        }
    }
}
