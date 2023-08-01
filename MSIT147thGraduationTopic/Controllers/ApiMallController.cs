using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using NuGet.Versioning;
using System.Linq;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiMallController : Controller
    {
        private readonly GraduationTopicContext _context;
        public ApiMallController(GraduationTopicContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult DisplaySearchResult(
            string txtKeyword, int searchCondition, int displayorder, int pageSize, int PageIndex
            ) //todo 串接側邊選單類別搜尋 , int? minPrice, int? maxPrice, int? sideCategoryId 用AJAX生成
        {
            IEnumerable<MallDisplay> datas = _context.MallDisplays   //僅顯示上架商品
                .Where(md => md.Display == true).Where(md => md.OnShelf == true);

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

            datas = displayorder switch
            {
                0 => datas.OrderByDescending(md => md.SpecId),  //最新商品
                1 => datas.OrderBy(md => md.SpecId),            //由舊到新
                2 => datas.OrderBy(md => md.Price),             //價格由低至高
                3 => datas.OrderByDescending(md => md.Price),   //價格由高至低
                _ => datas.OrderByDescending(md => md.SpecId)
            };

            var contentofThisPage = datas.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(contentofThisPage);
        }

        [HttpGet]
        public IActionResult GetSearchResultLength(
            string txtKeyword, int searchCondition, int displayorder, int pageSize, int PageIndex
            ) //todo 串接側邊選單類別搜尋 , int? minPrice, int? maxPrice, int? sideCategoryId 用AJAX生成
        {
            IEnumerable<MallDisplay> datas = _context.MallDisplays
                .Where(md => md.Display == true).Where(md => md.OnShelf == true);

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

            datas = displayorder switch
            {
                0 => datas.OrderByDescending(md => md.SpecId),
                1 => datas.OrderBy(md => md.SpecId),
                2 => datas.OrderBy(md => md.Price),
                3 => datas.OrderByDescending(md => md.Price),
                _ => datas.OrderByDescending(md => md.SpecId)
            };

            var resultLength = datas.Count();

            return Json(resultLength);
        }
    }
}
