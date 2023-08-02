using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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
        public IActionResult EIndex(int id)
        {
            var model = (from x in _context.EvaluationInputs
                         join y in _context.Specs on x.MerchandiseId equals y.MerchandiseId
                         join z in _context.OrderLists on x.OrderId equals z.OrderId
                         where x.OrderId == id
                         select new EvaluationVM
                         { 
                             OrderId = x.OrderId,
                             MerchandiseId = x.MerchandiseId,
                             MerchandiseName = x.MerchandiseName,
                             SpecId = y.SpecId,
                             SpecName = y.SpecName,
                             //Comment = x.Comment,
                             //Score = x.Score,
                         })

                         //.GroupBy(e => new { e.Comment,e.Score })  //按此條件分組
                         //.Select(a => a.First())   //選許所有組別的第一個
                         .Distinct()
                         .ToList();
           
            if (model.Count == 0)
            {
                ViewBag.ErrorMessage = $"沒有訂單 ( {id} ) 資料";
            }

            return View(model);
        }

        [HttpPost]  
        public IActionResult EIndex(int id, List<Comments> comments)
        {
            List<Evaluation> datalist = new List<Evaluation>();
            
            foreach (var item in comments)
            {
                var data = new Evaluation();
                data.OrderId = id;
                data.MerchandiseId = item.MerchandiseId;
                data.Comment= item.Comment;
                data.Score = item.Score;

                datalist.Add(data);              
            }
            
            _context.Evaluations.AddRange(datalist);
            _context.SaveChanges();

            
            return RedirectToAction("Edit", "Evaluation", new { id });   //轉跳至訂單頁          
        }

        //public IActionResult Edit(int id)
        //{
        //    if (id != null)
        //        return RedirectToAction("EIndex");

        //    var evaluation = _context.Evaluations.FirstOrDefault(e => e.EvaluationId == id);

        //    if (evaluation != null)
        //    {
        //        _context.Evaluations.Remove(evaluation);
        //        _context.SaveChanges();
        //    }
        //    //foreach (var evaluation in model)
            //{
            //    evaluation.StarImageUrl = GetScore(evaluation.Score);
            //}



            //return View("EIndex", model);
        //}

        //private string GetScore(int score)
        //{
        //    const string GetScoreIndex = "/images/";
        //    switch (score)
        //    {
        //        case 1:
        //            return GetScoreIndex + "一星.png";
        //        case 2:
        //            return GetScoreIndex + "二星.png";
        //        case 3:
        //            return GetScoreIndex + "三星.png";
        //        case 4:
        //            return GetScoreIndex + "四星.png";
        //        case 5:
        //            return GetScoreIndex + "五星.png";
        //        default:
        //            return GetScoreIndex + "空星.png";
        //    }
        //}


    }
}
//for (int i = 0; i < comments.Count; i++)
//{
//    string comment = comments[i].Comment;
//    var score = comments[i].Score;
//}