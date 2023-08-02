using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
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
            string txtKeyword, int searchCondition, int displayorder, int pageSize, int PageIndex,
            int sideCategoryId, int? minPrice, int? maxPrice)
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

            var contentofThisPage = datas.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(contentofThisPage);
        }

        [HttpGet]
        public IActionResult GetSearchResultLength(
            string txtKeyword, int searchCondition, int pageSize, int PageIndex,
            int sideCategoryId, int? minPrice, int? maxPrice)
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

            datas = (sideCategoryId == 0) ? datas : datas.Where(md => md.CategoryId == sideCategoryId);
            datas = (minPrice.HasValue)?datas.Where(sp => sp.Price >= minPrice):datas;
            datas = (maxPrice.HasValue)?datas.Where(sp => sp.Price <= maxPrice):datas;

            var resultLength = datas.Count();

            return Json(resultLength);
        }

        [HttpGet]
        public IActionResult GenerateSideCategoryOptions(
            string txtKeyword, int searchCondition, int? minPrice, int? maxPrice)
        {
            var categoriesFromEF = _context.Categories.OrderBy(c => c.CategoryId);

            IEnumerable<MallDisplay> selectedProducts = _context.MallDisplays
                .Where(md => md.Display == true).Where(md => md.OnShelf == true);

            if (!string.IsNullOrEmpty(txtKeyword))
            {
                selectedProducts = searchCondition switch
                {
                    1 => selectedProducts.Where(sp => sp.FullName.Contains(txtKeyword)),
                    2 => selectedProducts.Where(sp => sp.BrandName.Contains(txtKeyword)),
                    3 => selectedProducts.Where(sp => sp.CategoryName.Contains(txtKeyword)),
                    _ => selectedProducts
                };
            }
            if (minPrice.HasValue) selectedProducts = selectedProducts.Where(sp => sp.Price >= minPrice);
            if (maxPrice.HasValue) selectedProducts = selectedProducts.Where(sp => sp.Price <= maxPrice);

            List<CategoryVM> datas = new List<CategoryVM>();
            CategoryVM data_0 = new CategoryVM()
            {
                CategoryId = 0,
                CategoryName = "不限類別",
                matchedMerchandiseNumber = selectedProducts.Count()
            };
            datas.Add(data_0);

            foreach (var cFEF in categoriesFromEF)
            {
                CategoryVM data = new CategoryVM();
                data.category = cFEF;

                data.matchedMerchandiseNumber = selectedProducts
                    .Where(rC => rC.CategoryName == cFEF.CategoryName).Count();
                datas.Add(data);
            }

            return Json(datas);
        }

    }
}
