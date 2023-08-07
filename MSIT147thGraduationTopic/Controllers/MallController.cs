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

        public IActionResult Index(/*string txtKeyword = "", int searchCondition = 1, int sideCategoryId = 0*/)//todo 頁籤顯示數量不正常
        {
            //IEnumerable<MallDisplay> datas = _context.MallDisplays
            //    .Where(md => md.Display == true).Where(md => md.OnShelf == true);

            //if (!string.IsNullOrEmpty(txtKeyword))
            //{
            //    datas = searchCondition switch
            //    {
            //        1 => datas.Where(md => md.FullName.Contains(txtKeyword)),
            //        2 => datas.Where(md => md.BrandName.Contains(txtKeyword)),
            //        3 => datas.Where(md => md.CategoryName.Contains(txtKeyword)),
            //        _ => datas
            //    };
            //}

            //datas = (sideCategoryId == 0) ? datas : datas.Where(md => md.CategoryId == sideCategoryId);

            // todo 連結至購物車
            // todo 寵物類別TAG查詢
            return View();
        }

        public IActionResult Viewpage(int MerchandiseId)
        {
            IEnumerable<Spec> datas = _context.Specs
                .Where(s => s.MerchandiseId == MerchandiseId).Where(s => s.OnShelf == true);

            return View(datas);
        }
    }
}
