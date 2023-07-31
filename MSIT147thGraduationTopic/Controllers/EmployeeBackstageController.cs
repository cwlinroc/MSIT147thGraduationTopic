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
using Microsoft.Extensions.Options;
using MSIT147thGraduationTopic.Models.Infra.Utility;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EmployeeBackstageController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly IOptions<OptionSettings> _options;
        private readonly string[] _employeeRoles;
        private readonly EmployeeService _service;

        public EmployeeBackstageController(GraduationTopicContext context
            , IWebHostEnvironment environment
            , IOptions<OptionSettings> options)
        {
            _context = context;
            _options = options;
            _employeeRoles = options.Value.EmployeeRoles!;
            _service = new EmployeeService(context, environment, _employeeRoles);
        }
        public IActionResult Index()
        {
            return View(_employeeRoles);
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult Welcome()
        {
            string userName = HttpContext.User.FindFirstValue("UserName");
            string role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            return View((userName, role));
        }


        //TODO-cw 分層移位置
        public record LogInRecord([Required] string account, [Required] string password);
        [HttpPost]
        public async Task<IActionResult> ChangeAccount(LogInRecord record)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var employee = _context.Employees.FirstOrDefault(o => o.EmployeeAccount == record.account);
            if (employee == null) return Json(false);

            var saltedPassword = record.password.GetSaltedSha256(employee.Salt);
            if (employee.EmployeePassword != saltedPassword) return Json(false);

            var role = _employeeRoles[employee.Permission - 1];

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
