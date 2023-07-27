using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Xml.Linq;
using static MSIT147thGraduationTopic.Models.ViewModels.EvaluationVM;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EvaluationController : Controller
    {
        private readonly GraduationTopicContext db = new GraduationTopicContext();



        //以OrderId撈訂單資料
        public IActionResult EIndex(int OrderId)
        {
            var model = from x in db.EvaluationInputs
                        where x.OrderId == OrderId
                        select new EvaluationVM
                        {
                            OrderId = x.OrderId,
                            MerchandiseId = x.MerchandiseId,
                            SpecId = x.SpecId
                        };           
            return View(model);
        }
        
        [HttpPost]  
        public IActionResult EIndex(int OrderId,List<Comments> comments)
        {
            foreach (var item in comments)
            {
                var data = new Evaluation();
                data.OrderId = OrderId;
                data.MerchandiseId = item.MerchandiseId;
                data.Comment= item.Comment;
                data.Score = item.Score;
            }
            return View();
        }


    }
}
//for (int i = 0; i < comments.Count; i++)
//{
//    string comment = comments[i].Comment;
//    var score = comments[i].Score;
//}