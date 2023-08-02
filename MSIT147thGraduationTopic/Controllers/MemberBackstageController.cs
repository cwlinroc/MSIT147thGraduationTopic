using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using MSIT147thGraduationTopic.Models.Services;
using System.Data;

namespace MSIT147thGraduationTopic.Controllers
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

        //[Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
