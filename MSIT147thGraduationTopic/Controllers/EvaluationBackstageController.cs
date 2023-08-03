using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Data;
using System.Linq;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EvaluationBackstageController : Controller
    {
        private readonly GraduationTopicContext _context;
        
        public EvaluationBackstageController(GraduationTopicContext context)
        {
            _context = context;
        }

        public IActionResult EBIndex()
        {
            
            return View();

        }
        
        [HttpPost]
        public IActionResult EBIndex(string keyword)
        {
            var model = from e in _context.Evaluations
                        where e.OrderId.ToString().Contains(keyword) ||
                              e.Merchandise.MerchandiseName.Contains(keyword) ||
                              e.Comment.Contains(keyword)
                        select new EvaluationVM
                        {
                            EvaluationId = e.EvaluationId,
                            OrderId = e.OrderId,
                            MerchandiseName = e.Merchandise.MerchandiseName,
                            Score = e.Score,
                            Comment = e.Comment,
                        };

            //var model = _context.Evaluations
            //            .Where(x => string.IsNullOrEmpty(keyword) || x.Merchandise.MerchandiseName.Contains(keyword))
            //            .Select(x => new EvaluationVM
            //            {
            //                OrderId = x.OrderId,
            //                MerchandiseId = x.MerchandiseId,
            //                Score = x.Score,
            //                Comment = x.Comment,
            //            }).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var evaluation = _context.Evaluations.FirstOrDefault(e => e.EvaluationId == id);

            if (evaluation != null)
            {
                _context.Evaluations.Remove(evaluation);
                _context.SaveChanges();
            }

            return RedirectToAction("EBIndex");
        }

        public ActionResult PageList(int pageSize = 25, int pageNo = 1)
        {
            
            using (var dbContext = new GraduationTopicContext())
            {
                var sql = @"
                        DECLARE @pageSize INT, @pageNo INT;
                        SET @pageSize = @p0;
                        SET @pageNo = @p1;
                        ;WITH T
                        AS (
                            SELECT *
                            FROM Evaluations.EvaluationsId                            
                        )
                        SELECT TotalCount = COUNT(1) OVER (), T.*
                        FROM T
                        ORDER BY EvaluationsId
                        OFFSET(@pageNo - 1) * @pageSize ROWS
                        FETCH NEXT @pageSize ROWS ONLY;
        ";

                // 執行分頁查詢
                var query = dbContext.Database.SqlQuery<Evaluations>(sql, pageSize, pageNo).ToList();

                // 獲取總記錄數
                var totalCount = dbContext.Evaluations.Count(p => p.EvaluationId > 10);

                // 傳遞查詢結果和總記錄數到View中
                ViewBag.PageNo = pageNo;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalCount = totalCount;
                return View(query);
            }
        }

    }

}
