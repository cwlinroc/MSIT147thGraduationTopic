using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using Microsoft.AspNetCore.Authorization;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EmployeeBackstageController : Controller
    {
        //TODO add in appsettings
        private readonly string[] _roles = { "管理員", "經理", "員工" };
        private readonly GraduationTopicContext _context;
        private readonly EmployeeService _service;

        public EmployeeBackstageController(GraduationTopicContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _service = new EmployeeService(context, environment);
        }
        public IActionResult Index()
        {
            return View(_roles);
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult ChangeAccount()
        {
            string userName = HttpContext.User.FindFirstValue("UserName");
            string role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            return View((userName, role));
        }


        //TODO 分層移位置
        public record LogInRecord([Required] string account, [Required] string password);
        [HttpPost]
        public async Task<IActionResult> ChangeAccount(LogInRecord record)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var employee = _context.Employees.FirstOrDefault(o => o.EmployeeAccount == record.account);
            if (employee == null) return Json(false);

            var saltedPassword = record.password.GetSaltedSha256(employee.Salt);
            if (employee.EmployeePassword != saltedPassword) return Json(false);

            var role = _roles[employee.Permission - 1];

            var claims = new List<Claim>
                        {
                            new (ClaimTypes.Name, employee.EmployeeAccount),
                            new ("UserName", employee.EmployeeName),
                            new ("AvatarName", employee.AvatarName??""),
                            new (ClaimTypes.Email, employee.EmployeeEmail),
                            new (ClaimTypes.Role, role)
                        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Json(true);
        }

    }
}
