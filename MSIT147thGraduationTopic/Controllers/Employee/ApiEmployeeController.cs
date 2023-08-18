using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;

namespace MSIT147thGraduationTopic.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEmployeeController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly EmployeeService _service;
        private readonly IWebHostEnvironment _environment;
        private readonly string[] _employeeRoles;

        public ApiEmployeeController(GraduationTopicContext context
            , IWebHostEnvironment environment
            , IOptions<OptionSettings> options)
        {
            _context = context;
            _environment = environment;
            _employeeRoles = options.Value.EmployeeRoles!;
            _service = new EmployeeService(context, environment, options.Value.EmployeeRoles!);
        }

        [HttpGet]
        [Authorize(Roles = "管理員,經理")]
        public ActionResult<List<EmployeeVM>> GetAllEmployees()
        {
            return _service.GetAllEmployees().ToList();
        }

        [HttpGet("{query}")]
        [Authorize(Roles = "管理員,經理")]
        public ActionResult<List<EmployeeVM>> GetEmployeesByNameOrAccount(string query)
        {
            return _service.queryEmployeesByNameOrAccount(query).ToList();
        }

        [HttpPost]
        [Authorize(Roles = "管理員,經理")]
        public ActionResult<int> CreateEmployee([FromForm] EmployeeCreateVM vm, [FromForm] IFormFile? avatar)
        {
            var employeeId = _service.CreateEmployee(vm.ToDto(), avatar);

            return employeeId;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "管理員,經理")]
        public ActionResult<int> UpdateEmployee([FromForm] EmployeeEditDto dto, int id, [FromForm] IFormFile? avatar)
        {
            var employeeId = _service.EditEmployee(dto, id, avatar);

            return employeeId;
        }

        [HttpGet("hassamenameaccount")]
        [Authorize(Roles = "管理員,經理")]
        public async Task<ActionResult<bool>> HasSameNameAccount(string account)
        {
            return (await _context.Employees.AnyAsync(o => o.EmployeeAccount == account) 
                || await _context.Members.AnyAsync(o => o.Account == account)) ;
        }

        public record Container([Required] string Permission);

        [HttpPut("permission/{id}")]
        [Authorize(Roles = "管理員")]
        public ActionResult<int> UpdateEmployeePermission(Container permission, int id = 0)
        {
            var employeeId = _service.ChangeEmployeePermission(id, permission.Permission);

            return employeeId;
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "管理員")]
        public ActionResult<int> DeleteEmployee(int id)
        {
            return _service.DeleteEmployee(id);
        }


        public record LogInRecord([Required] string account, [Required] string password);

        [HttpPost("login")]
        public async Task<ActionResult<bool>> ChangeAccount([FromForm] LogInRecord record)
        {
            if (HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            var employee = _context.Employees.FirstOrDefault(o => o.EmployeeAccount == record.account);
            if (employee == null) return false;

            var saltedPassword = record.password.GetSaltedSha256(employee.Salt);
            if (employee.EmployeePassword != saltedPassword) return false;

            var role = _employeeRoles[employee.Permission - 1];

            var claims = new List<Claim>
                        {
                            new (ClaimTypes.Name, employee.EmployeeAccount),
                            new ("UserName", employee.EmployeeName),
                            new ("AvatarName", employee.AvatarName??""),
                            new Claim("EmployeeId", employee.EmployeeId.ToString()),
                            new (ClaimTypes.Email, employee.EmployeeEmail),
                            new (ClaimTypes.Role, role)
                        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return true;
        }

        public record ConfirmRecord([Required] string Password);
        [HttpPost("confirm")]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<ActionResult<bool>> ConfirmPassword(ConfirmRecord record)
        {
            if (!HttpContext.User.Identity?.IsAuthenticated ?? false) return false;

            var test = HttpContext.User.FindFirstValue("EmployeeId");

            if (!int.TryParse(HttpContext.User.FindFirstValue("EmployeeId"), out int employeeId)) return false;

            bool correction = await _service.ConfirmWithPassword(employeeId, record.Password);

            return correction;
        }


        [HttpGet("selfavatar")]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<ActionResult<string>> GetSelfAvatar()
        {
            if (!int.TryParse(HttpContext.User.FindFirstValue("EmployeeId"), out int employeeId))
            {
                return BadRequest("找不到對應員工ID");
            }
            return await _service.GetAvatarName(employeeId);
        }



        [HttpPost("selfavatar")]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<ActionResult<int>> UploadSelfAvatar(IFormFile? image)
        {
            if (image == null) return -1;
            if (!int.TryParse(HttpContext.User.FindFirstValue("EmployeeId"), out int employeeId))
            {
                return BadRequest("找不到對應員工ID");
            }
            return await _service.UpdateAvatar(employeeId, image);
        }

    }
}
