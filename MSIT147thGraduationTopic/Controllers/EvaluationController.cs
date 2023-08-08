using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Security.Claims;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static MSIT147thGraduationTopic.Models.ViewModels.EvaluationVM;
using static System.Formats.Asn1.AsnWriter;

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
        //Evaluations >判斷>有-修改/無-新增
        public IActionResult EIndex(int id)
        {
            var model = (from o in _context.OrderLists
                         join s in _context.Specs on  o.SpecId  equals s.SpecId 
                         join m in _context.Merchandises on new { s.MerchandiseId } equals new { m.MerchandiseId }
                         where o.OrderId == id
                         select new EvaluationVM
                         {
                             OrderId = o.OrderId,
                             MerchandiseId = m.MerchandiseId,
                             MerchandiseName = m.MerchandiseName,
                             SpecId = s.SpecId,
                             SpecName = s.SpecName,                                                   
                         })
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
                data.SpecId = item.SpecId;
                data.MerchandiseId = item.MerchandiseId;
                data.Comment = item.Comment;
                data.Score = item.Score;

                datalist.Add(data);              
            }
            _context.Evaluations.AddRange(datalist);
            _context.SaveChanges();

            
            return RedirectToAction("Edit", "Evaluation", new { id });   //todo轉跳至訂單頁          
        }

        public IActionResult Edit(int id)
        {
            
            var model = (from e in _context.Evaluations
                         join s in _context.Specs on e.SpecId equals s.SpecId
                         join m in _context.Merchandises on new { e.MerchandiseId } equals new { m.MerchandiseId }
                         where e.OrderId == id
                         select new EvaluationVM
                         {
                             OrderId = e.OrderId,
                             MerchandiseId = e.MerchandiseId,
                             MerchandiseName = m.MerchandiseName,
                             SpecId = e.SpecId,
                             SpecName = s.SpecName,
                             Comment = e.Comment,
                             Score = e.Score,
                         })
                         .ToList();
            return View("EIndex", model);
        }

        [HttpPost]
        public IActionResult Edit(int id, List<Comments> comments)
        {
            List<Evaluation> datalist = new List<Evaluation>();

            foreach (var item in comments)
            {
                var data = new Evaluation();
                data.OrderId = id;
                data.MerchandiseId = item.MerchandiseId;
                data.Comment = item.Comment;
                data.Score = item.Score;

                var haveEvaluation = _context.Evaluations.FirstOrDefault(e => e.EvaluationId == id && e.MerchandiseId == item.MerchandiseId);  //SpecId??
                if (haveEvaluation != null)
                {
                    haveEvaluation.Comment = data.Comment;
                    haveEvaluation.Score = data.Score;
                    _context.Evaluations.Update(haveEvaluation);
                }
                else
                { 
                    _context.Evaluations.Add(data);
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Edit", "Evaluation", new { id });   //todo轉跳至訂單頁          
        }





        //測試用會員購買紀錄
        public IActionResult Test()
        {
            int memberId;
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out memberId))
            {
                memberId = 1;
            }
            var orderIds = _context.Orders.Where(o => o.MemberId == memberId).Select(o=>o.OrderId).ToList();

            return View(orderIds);
        }
        //測試用會員購買紀錄
    }
}
