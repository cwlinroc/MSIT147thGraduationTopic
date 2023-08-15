using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.ViewComponents
{
    public class EvaluationVC : ViewComponent
    {

        private readonly GraduationTopicContext _context;

        public EvaluationVC(GraduationTopicContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)   //商品id
        {
            //ViewData["age"] = 20;
            //ViewBag.name = "測試";
            //return View();
            ViewData["MerchandiseId"] = id;
            var merchandiseEvaluation = _context.Evaluations.FirstOrDefault(e => e.MerchandiseId == id);

            if (merchandiseEvaluation != null)   //Evaluations有留言資料,帶出
            {
                var model = (from e in _context.EvaluationInputs
                             join s in _context.Specs on e.SpecId equals s.SpecId
                             join m in _context.Merchandises on new { e.MerchandiseId } equals new { m.MerchandiseId }
                             where e.MerchandiseId == id
                             select new EvaluationVM
                             {
                                 MerchandiseName = m.MerchandiseName,
                                 SpecName = s.SpecName,
                                 Comment = e.Comment,
                                 Score = e.Score,
                             }).Take(5)
                        .ToList();
                return View(model);
            }

            ViewBag.noEvaluation = "此商品尚未有評論";
            return View(new List<EvaluationVM>());
        }
         

    }
}
