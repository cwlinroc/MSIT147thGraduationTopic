using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using MSIT147thGraduationTopic.Models.ViewModels;
using NuGet.Versioning;
using System;
using System.Linq;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Azure;
using MSIT147thGraduationTopic.Models.Infra.Repositories;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiMallController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly MallRepository _repo;
        public ApiMallController(GraduationTopicContext context)
        {
            _context = context;
            _repo = new MallRepository(context);
        }

        [HttpGet]
        public IActionResult DisplaySearchResult(
            string txtKeyword, int searchCondition, int displayorder, int pageSize, int PageIndex,
            int sideCategoryId, int? minPrice, int? maxPrice, int tagId = 0)
        {
            //保存參數以供換頁時保留設定
            if (!string.IsNullOrEmpty(txtKeyword)) HttpContext.Response.Cookies.Append("Mall_txtKeyword", txtKeyword);
            if (string.IsNullOrEmpty(txtKeyword)) HttpContext.Response.Cookies.Append("Mall_txtKeyword", "");
            HttpContext.Response.Cookies.Append("Mall_searchCondition", searchCondition.ToString());
            HttpContext.Response.Cookies.Append("Mall_displayorder", displayorder.ToString());
            HttpContext.Response.Cookies.Append("Mall_pageSize", pageSize.ToString());

            IEnumerable<MallDisplay> datas = _repo.getBasicMallDisplay(txtKeyword, searchCondition, minPrice, maxPrice);

            datas = (sideCategoryId == 0) ? datas : datas.Where(md => md.CategoryId == sideCategoryId);
            if (tagId != 0)
            {
                IQueryable<int> thisTag = _context.SpecTags.Where(st => st.TagId == tagId).Select(st => st.SpecId);
                datas = datas.Where(md => thisTag.Contains(md.SpecId));
            }

            datas = displayorder switch
            {
                0 => datas.OrderByDescending(md => md.SpecId),      //最新商品
                1 => datas.OrderBy(md => md.SpecId),                //由舊到新
                2 => datas.OrderByDescending(md => md.Popularity),  //熱門商品
                3 => datas.OrderBy(md => md.Price),                 //價格由低至高
                4 => datas.OrderByDescending(md => md.Price),       //價格由高至低
                _ => datas.OrderByDescending(md => md.SpecId)
            };

            var contentofThisPage = datas.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();

            IEnumerable<MallDisplayVM> datasWithScore = contentofThisPage
                            .Select(md => new MallDisplayVM
                            {
                                malldisplay = md,
                                Score = (_context.Evaluations.Where(e => e.SpecId == md.SpecId).Any())
                                ? _context.Evaluations.Where(e => e.SpecId == md.SpecId).Average(e => e.Score) : 0
                            }).ToList();

            return Json(datasWithScore);
        }

        [HttpGet]
        public IActionResult GetSearchResultLength(
            string txtKeyword, int searchCondition, int? minPrice, int? maxPrice, int sideCategoryId = 0, int tagId = 0)
        {
            IEnumerable<MallDisplay> datas = _repo.getBasicMallDisplay(txtKeyword, searchCondition, minPrice, maxPrice);
            
            datas = (sideCategoryId == 0) ? datas : datas.Where(md => md.CategoryId == sideCategoryId);
            
            if (tagId != 0)
            {
                IQueryable<int> thisTag = _context.SpecTags.Where(st => st.TagId == tagId).Select(st => st.SpecId);
                datas = datas.Where(md => thisTag.Contains(md.SpecId));
            }

            var resultLength = datas.Count();

            return Json(resultLength);
        }

        [HttpGet]
        public IActionResult GenerateSideCategoryOptions(
            string txtKeyword, int? minPrice, int? maxPrice, int searchCondition = 0, int tagId = 0)
        {
            var categoriesFromEF = _context.Categories.OrderBy(c => c.CategoryId);

            IEnumerable<MallDisplay> selectedProducts = _repo.getBasicMallDisplay(txtKeyword, searchCondition, minPrice, maxPrice);
            
            if (tagId != 0)
            {
                IQueryable<int> thisTag = _context.SpecTags.Where(st => st.TagId == tagId).Select(st => st.SpecId);
                selectedProducts = selectedProducts.Where(md => thisTag.Contains(md.SpecId));
            }

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

        [HttpGet]
        public IActionResult GenerateSideTagOptions(
            string txtKeyword, int? minPrice, int? maxPrice, int searchCondition = 0, int sideCategoryId = 0)
        {
            var tagsFromEF = _context.Tags.OrderBy(c => c.TagId);

            IEnumerable<MallDisplay> selectedProducts = _repo.getBasicMallDisplay(txtKeyword, searchCondition, minPrice, maxPrice);

            selectedProducts = (sideCategoryId == 0) ? selectedProducts : selectedProducts.Where(md => md.CategoryId == sideCategoryId);

            List<TagVM> tags = new List<TagVM>();
            TagVM tag_0 = new TagVM()
            {
                TagId = 0,
                TagName = "不限",
                matchedMerchandiseNumber = selectedProducts.Count()
            };
            tags.Add(tag_0);

            //前後台寵物類型名稱用詞不同，因此需另外輸入
            string[] TagNames = new string[] { "貓咪", "狗狗", "鼠寶", "兔寶" };
            for (int i = 1; i <=4; i++)
            {
                TagVM tag = new TagVM()
                {
                    TagId = i,
                    TagName = TagNames[i - 1]
                };
                IQueryable<int> matchedSpec = _context.SpecTags.Where(st => st.TagId == i).Select(st => st.SpecId);
                tag.matchedMerchandiseNumber = selectedProducts.Where(md => matchedSpec.Contains(md.SpecId)).Count();
                tags.Add(tag);
            }

            return Json(tags);
        }

        [Authorize(Roles = "會員")]
        [HttpPost]
        public async Task<IActionResult> AddtoCart(int SpecId, int Quantity = 1)
        {
            int memberId = int.Parse(HttpContext.User.FindFirstValue("MemberId"));
            bool isSuccess = false;

            if (ModelState.IsValid)
            {
                //驗證購物車內是否已有規格
                IEnumerable<CartItem> cartItem = _context.CartItems
                            .Where(ci => ci.MemberId == memberId).Where(ci => ci.SpecId == SpecId);
                bool cartHasItem = cartItem.Any();

                //有 => 更新
                if (cartHasItem)
                {
                    int Amount = _context.Specs.Where(s =>  s.SpecId == SpecId).Select(s => s.Amount).First();

                    CartItem thisCartItem = cartItem.First();
                    int thisQuantity = thisCartItem.Quantity;
                    //驗證不可超過庫存數
                    thisCartItem.Quantity = (thisQuantity + Quantity > Amount) ? Amount : thisQuantity + Quantity;

                    _context.Update(thisCartItem);
                }

                //無 => 新增
                if (!cartHasItem)
                {
                    CartItem ci = new CartItem()
                    {
                        MemberId = memberId,
                        SpecId = SpecId,
                        Quantity = Quantity
                    };

                    _context.Add(ci);
                }

                await _context.SaveChangesAsync();
                isSuccess = true;
                return Json(isSuccess);
            }
            return Json(isSuccess);
        }
    }
}
