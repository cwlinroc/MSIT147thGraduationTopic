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
            var or
            //var model = (from x in _context.EvaluationInputs   
            //             where x.OrderId == id
            //             select new EvaluationVM
            //            {
            //                OrderId = x.OrderId,                            
            //                MerchandiseId = x.MerchandiseId,
            //                MerchandiseName = x.MerchandiseName,
            //                SpecId = x.SpecId,
            //                SpecName= x.SpecName,                                                     
            //            }).ToList();

            var model = (from x in _context.Evaluations
                          join s in _context.Specs on x.MerchandiseId equals s.MerchandiseId
                          where x.OrderId == id
                          select new EvaluationVM
                          {
                              OrderId = x.OrderId,
                              MerchandiseId = x.MerchandiseId,
                              MerchandiseName = x.Merchandise.MerchandiseName,
                              SpecId = s.SpecId,
                              SpecName = s.SpecName,
                          })
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

            
            return RedirectToAction("Edit", "Evaluation", new { id });
            //return RedirectToAction("EIndex", "Evaluation");
        }

        public IActionResult Edit(int id)
        {
            var model = (from x in _context.Evaluations
                         where x.OrderId == id
                         select new EvaluationVM
                         {
                             OrderId = x.OrderId,
                             MerchandiseId = x.MerchandiseId,
                             Comment = x.Comment,
                             Score = x.Score                                                        
                         }).ToList();

            if (model.Count == 0)
            {
                ViewBag.ErrorMessage = $"沒有訂單 ( {id} ) 資料";
            }

            return View("EIndex", model);
        }


    }
}
//for (int i = 0; i < comments.Count; i++)
//{
//    string comment = comments[i].Comment;
//    var score = comments[i].Score;
//}