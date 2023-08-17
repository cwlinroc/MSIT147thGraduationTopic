using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.Controllers;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.ViewModels;
using NuGet.Protocol;
using System.Text.Json;

namespace MSIT147thGraduationTopic.ViewComponents
{
    public class EvaluationVC : ViewComponent
    {

        private readonly GraduationTopicContext _context;
        private readonly ApiEvaluationController _api;

        public EvaluationVC(GraduationTopicContext context)
        {
            _context = context;
            _api = new ApiEvaluationController(context);
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)   //商品id
        {           
            ViewData["MerchandiseId"] = id;
            var merchandiseEvaluation = _context.EvaluationInputs.FirstOrDefault(e => e.MerchandiseId == id);

            if (merchandiseEvaluation != null)   //Evaluations有留言資料,帶出
            {
                
                var evaluationPageCounts = 1;  //頁數

                var Jsonmodel = await _api.ShowMoreEvaliation(id, evaluationPageCounts);
                var model = ((Microsoft.AspNetCore.Mvc.JsonResult)Jsonmodel).Value as List<EvaluationVM>;

                return View("Default", model);
            }

            ViewBag.noEvaluation = "此商品尚未有評論";
            return View("Default",new List<EvaluationVM>());
        }
         

    }
}
