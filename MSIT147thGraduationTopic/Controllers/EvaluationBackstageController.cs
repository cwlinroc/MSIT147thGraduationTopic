using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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

        public IActionResult EBIndex(string keyword, int pageSize, int pageNo,int totalCount)
        {
            pageSize = 5;
            if (keyword == null)
                return View();

            var query = PerformSqlQuery(pageSize, pageNo, keyword);

            // 獲取帶出資料總記錄數
            ViewBag.TotalPage = (totalCount % pageSize) > 0 ? (totalCount / pageSize) + 1 : (totalCount / pageSize);
            ViewBag.TotalCount = totalCount;            
            // 傳遞查詢結果和總記錄數到View中
            ViewBag.keyword = keyword;
            ViewBag.PageNo = pageNo;
            ViewBag.PageSize = pageSize;

            return View(query.Select(e => new EvaluationVM
            {
                EvaluationId = e.EvaluationId,
                OrderId = e.OrderId,
                MerchandiseName = e.MerchandiseName,
                Score = e.Score,
                Comment = e.Comment,
            }));
        }
        
        [HttpPost]
        public IActionResult EBIndex(string keyword)
        {
            var pageSize = 5; 
            var pageNo = 1;
            keyword = !string.IsNullOrEmpty(keyword) ? keyword : "NULL";
            var model = from e in _context.EvaluationInputs
                        where e.OrderId.ToString().Contains(keyword) ||
                              e.MerchandiseName.Contains(keyword) ||
                              e.Comment.Contains(keyword)

                        select new EvaluationVM
                        {
                            EvaluationId = e.EvaluationId,
                            OrderId = e.OrderId,
                            MerchandiseName = e.MerchandiseName,
                            Score = e.Score,
                            Comment = e.Comment,
                        };
            model = model.OrderByDescending(e => e.EvaluationId);

            ////var model = _context.Evaluations
            ////            .Where(x => string.IsNullOrEmpty(keyword) || x.Merchandise.MerchandiseName.Contains(keyword))
            ////            .Select(x => new EvaluationVM
            ////            {
            ////                OrderId = x.OrderId,
            ////                MerchandiseId = x.MerchandiseId,
            ////                Score = x.Score,
            ////                Comment = x.Comment,
            ////            }).ToList();

            var query = PerformSqlQuery(pageSize, pageNo, keyword);

            // 獲取帶出資料總記錄數
            //var totalCount = model.Count();
            var totalCount = _context.EvaluationInputs
                .Where(x=> x.OrderId.ToString().Contains(keyword)|| x.MerchandiseName.Contains(keyword)||x.Comment.Contains(keyword) )
                .Count();

            // 傳遞查詢結果和總記錄數到View中
            ViewBag.keyword = keyword;
            ViewBag.PageNo = pageNo;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPage = (totalCount % pageSize) > 0 ? (totalCount / pageSize) + 1 : (totalCount / pageSize); 
            

            // 當頁數據
            var currentPageData = query.Skip((pageNo-1) * pageSize).Take(pageSize).ToList();

            return View(currentPageData.Select(e => new EvaluationVM
            {
                EvaluationId = e.EvaluationId,
                OrderId = e.OrderId,
                MerchandiseName = e.MerchandiseName,
                Score = e.Score,
                Comment = e.Comment,
            }));
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
        private List<EvaluationInput> PerformSqlQuery(int pageSize, int pageNo, string keyword)
        {
            var sql = $@"
                        DECLARE @pageSize INT, @pageNo INT, @keyword NVARCHAR(255);
                        SET @pageSize = @p0;
                        SET @pageNo = @p1;
                        SET @keyword = @p2;
                        ;WITH T                               
                        AS (                            
                                SELECT *
                                FROM EvaluationInput e
                                WHERE
                                @keyword IS NULL OR
                                e.OrderId LIKE '%' + @keyword + '%' OR
                                e.MerchandiseName LIKE '%' + @keyword + '%' OR
                                e.Comment LIKE'%' + @keyword + '%'       
                                                             
                        )
                        SELECT TotalCount = COUNT(1) OVER (), T.*
                        FROM T
                        ORDER BY EvaluationId DESC
                        OFFSET(@pageNo - 1) * @pageSize ROWS
                        FETCH NEXT @pageSize ROWS ONLY;";
            //分頁查詢
            return _context.EvaluationInputs.FromSqlRaw<EvaluationInput>(sql, pageSize, pageNo, keyword).ToList();
        }
    }

}
