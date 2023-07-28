using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiSpecController : Controller
    {
        private readonly GraduationTopicContext _context;

        public ApiSpecController(GraduationTopicContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ShowMerchandise(int id)
        {
            var datas = _context.MerchandiseSearches.Where(m => m.MerchandiseId == id);

            return Json(datas);
        }

        [HttpPost]
        public IActionResult CheckforCreateSpec(SpecVM specvm)
        {
            bool[] package = new bool[2];

            package[0] = _context.Specs.Any(s => s.SpecName == specvm.SpecName);//todo 同一商品ID下才不可同名

            package[1] = false;
            if (specvm.photo != null)
            {
                if (!specvm.photo.ContentType.Contains("image")) package[1] = true;
            }
            //↓ photo為null時會造成photo.ContentType.Contains("image")有NullReference錯誤，因此無法使用三元運算
            //package[1] = (photo.ContentType != null && !photo.ContentType.Contains("image")) ? true : false;

            return Json(package);
        }
    }
}
