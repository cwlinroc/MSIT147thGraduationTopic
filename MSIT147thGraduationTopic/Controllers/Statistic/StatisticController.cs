using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MSIT147thGraduationTopic.Controllers.Statistic
{
    public class StatisticController : Controller
    {
        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Sales()
        {
            return View();
        }

        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Trends()
        {
            return View();
        }
        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Compare()
        {
            return View();
        }

        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Test()
        {
            return View();
        }


    }
}
