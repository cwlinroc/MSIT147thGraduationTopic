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

        public IActionResult Index()
        {
            // todo 用超連結重製搜尋條件(所有商品)
            // todo 改變當前分頁按鈕屬性
            // todo 無結果時顯示字樣
            // todo 連結至購物車
            // todo 寵物類別TAG查詢
            return View();
        }

        public IActionResult Viewpage(int SpecId)
        {
            IEnumerable<Spec> datas = _context.Specs
                .Where(s => s.SpecId == SpecId).Where(s => s.OnShelf == true);

            return View(datas);
        }
    }
}
