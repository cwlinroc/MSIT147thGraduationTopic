using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Xml.Linq;
using static MSIT147thGraduationTopic.Models.ViewModels.EvaluationVM;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EvaluationController : Controller
    {
        
        private readonly GraduationTopicContext _context;

        public EvaluationController(GraduationTopicContext context)
        {
            _context = context;
        }


        //以OrderId撈訂單資料
        public IActionResult EIndex(int OrderId)
        {
            //記得刪
            if (OrderId == 0) { OrderId = 1; }
            var model = (from x in _context.EvaluationInputs
                        where x.OrderId == OrderId
                        select new EvaluationVM
                        {
                            OrderId = x.OrderId,
                            MerchandiseId = x.MerchandiseId,
                            SpecId = x.SpecId
                        }).ToList();           
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