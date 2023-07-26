using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiMerchandiseController : Controller
    {
        private readonly GraduationTopicContext _context;

        public ApiMerchandiseController(GraduationTopicContext context)
        {
            _context = context;
        }

        public IActionResult Merchandises()
        {
            var datas = _context.Merchandises.OrderBy(a => a.MerchandiseId);

            return Json(datas);
        }

        public IActionResult GenerateBrandOptions()
        {
            var datas = _context.Brands.OrderBy(b => b.BrandId);

            return Json(datas);
        }

        public IActionResult GenerateCategoryOptions()
        {
            var datas = _context.Categories.OrderBy(c => c.CategoryId);

            return Json(datas);
        }

        [HttpPost]
        public IActionResult CheckforCreateMerchandise(MerchandiseVM merchandisevm)
        {
            bool[] package = new bool[2];

            package[0] = _context.Merchandises.Any(m => m.MerchandiseName == merchandisevm.MerchandiseName);

            package[1] = false;
            if (merchandisevm.photo != null)
            {
                if (!merchandisevm.photo.ContentType.Contains("image")) package[1] = true;
            }
            //↓ photo為null時會造成photo.ContentType.Contains("image")有NullReference錯誤，因此無法使用三元運算
            //package[1] = (photo.ContentType != null && !photo.ContentType.Contains("image")) ? true : false;

            return Json(package);
        }

        [HttpPost]
        public IActionResult CheckforEditMerchandise(MerchandiseVM merchandisevm)
        {
            bool[] package = new bool[2];

            package[0] = _context.Merchandises
                .Where(m => m.MerchandiseId != merchandisevm.MerchandiseId)
                .Any(m => m.MerchandiseName == merchandisevm.MerchandiseName);

            package[1] = false;
            if (merchandisevm.photo != null)
            {
                if (!merchandisevm.photo.ContentType.Contains("image")) package[1] = true;
            }

            return Json(package);
        }





        public IActionResult CheckSpecforDeleteMerchandise(int id)
        {
            var exists = _context.Specs.Any(s => s.SpecId == id);

            return Json(exists);
        }
    }
}
