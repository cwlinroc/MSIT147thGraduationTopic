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

namespace MSIT147thGraduationTopic.Controllers.Employee
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

        [Authorize(Roles = "管理員,經理")]
        public IActionResult Index()
        {
            return View(_employeeRoles);
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Welcome()
        {
            string userName = HttpContext.User.FindFirstValue("UserName");
            string role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            return View((userName, role));
        }


    }
}
