using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Data;

namespace MSIT147thGraduationTopic.Controllers.Member
{
    public class MemberBackstageController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly IOptions<OptionSettings> _options;
        private readonly string[] _employeeRoles;
        private readonly EmployeeService _service;

        public MemberBackstageController(GraduationTopicContext context
            , IWebHostEnvironment environment
            , IOptions<OptionSettings> options)
        {
            _context = context;
            _options = options;
            _employeeRoles = options.Value.EmployeeRoles!;
            _service = new EmployeeService(context, environment, _employeeRoles);
        }

        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult MemberList()
        {
            return View();
        }

        //[Authorize(Roles = "管理員,經理,員工")]
        //public IActionResult MemberList(int pageSize = 10, int pageIndex = 1)
        //{
        //    var query = PerformSqlQuery(pageSize, pageIndex);

        //    if (query == null)
        //        return View(new List<MemberVM>());

        //    // 獲取總記錄數
        //    var totalCount = _context.Members.Count(p => p.MemberId > 10);
        //    // 傳遞查詢結果和總記錄數到View中
        //    ViewBag.PageIndex = pageIndex;
        //    ViewBag.PageSize = pageSize;
        //    ViewBag.TotalCount = totalCount;

        //    return View(query.Select(e => new MemberVM
        //    {
        //        MemberId = e.MemberId,
        //        MemberName = e.MemberName,
        //        Gender = e.Gender ? "male" : "female",
        //        Account = e.Account,
        //        Phone = e.Phone,
        //        City = e.City,
        //        District = e.District,
        //        Address = e.Address,
        //        Email = e.Email,
        //    }));
        //}

        //private List<EFModels.Member> PerformSqlQuery(int pageSize, int pageIndex)
        //{
        //    var sql = @"
        //                DECLARE @pageSize INT, @pageIndex INT;
        //                SET @pageSize = @p0;
        //                SET @pageIndex = @p1;
        //                ;WITH T
        //                AS (
        //                    SELECT *
        //                    FROM Members
        //                )
        //                SELECT TotalCount = COUNT(1) OVER (), T.*
        //                FROM T
        //                ORDER BY MemberId
        //                OFFSET(@pageIndex - 1) * @pageSize ROWS
        //                FETCH NEXT @pageSize ROWS ONLY;";

        //    //分頁查詢
        //    return _context.Members.FromSqlRaw(sql, pageSize, pageIndex).ToList();
        //}
    }
}
