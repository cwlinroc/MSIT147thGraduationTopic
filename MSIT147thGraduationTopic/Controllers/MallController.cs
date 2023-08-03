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

        public IActionResult Index()//todo 關鍵字、條件、類別、Tag改成參數，頁數、排序保持AJAX
        {
            // todo 無結果時顯示字樣
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
