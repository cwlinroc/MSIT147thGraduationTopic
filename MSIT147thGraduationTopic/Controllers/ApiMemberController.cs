using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiMemberController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly MemberService _service;
        private readonly IWebHostEnvironment _environment;
        private readonly string[] _employeeRoles;

        public ApiMemberController(GraduationTopicContext context
            , IWebHostEnvironment environment
            , IOptions<OptionSettings> options)
        {
            _context = context;
            _environment = environment;
            _service = new MemberService(context, environment);

            _employeeRoles = options.Value.EmployeeRoles!;
        }

        [HttpGet]
        public ActionResult<List<MemberVM>> GetAllMembers()
        {
            return _service.GetAllMembers().ToList();
        }

        [HttpGet("{query}")]
        public ActionResult<List<MemberVM>> GetMembersByNameOrAccount(string query)
        {
            return _service.GetMembersByNameOrAccount(query).ToList();
        }

        [HttpPost]
        public ActionResult<int> CreateMember([FromForm] MemberCreateVM vm, [FromForm] IFormFile avatar)
        {
            var memberId = _service.CreateMember(vm.ToDto(), avatar);

            return memberId;
        }

        [HttpPut("{id}")]
        public ActionResult<int> UpdateMember([FromForm] MemberEditDto dto, int id, [FromForm] IFormFile avatar)
        {
            var memberId = _service.EditMember(dto, id, avatar);

            return memberId;
        }

        [HttpDelete("{id}")]
        public ActionResult<int> UpdateMember(int id)
        {
            return _service.DeleteMember(id);
        }

        //public record LoginRecord([Required] string Account, [Required] string Password, bool chkRemember);
        public record LoginRecord([Required] string Account, [Required] string Password, bool chkRemember);
        [HttpPost("login")]
        public async Task<ActionResult<string>> LogIn(LoginRecord record)
        {
            var emp = await _context.Employees
                                .FirstOrDefaultAsync(o => o.EmployeeAccount == record.Account);

            if (emp != null)
            {
                string saltedPassword = record.Password.GetSaltedSha256(emp.Salt);
                if (emp.EmployeePassword != saltedPassword) return string.Empty;

                var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, emp.EmployeeAccount),
                                new Claim("UserName", emp.EmployeeName),
                                new Claim("AvatarName", emp.AvatarName??""),
                                new Claim(ClaimTypes.Email, emp.EmployeeEmail),
                                new Claim(ClaimTypes.Role, _employeeRoles[emp.Permission-1])
                            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Url.Content("~/employeebackstage/welcome");
            }

            var member = await _context.Members
                    .Select(o => new { o.Account, o.Password, o.Salt, o.MemberName, o.Email, o.Avatar, o.MemberId })
                .FirstOrDefaultAsync(o => o.Account == record.Account);

            if (member != null)
            {
                string saltedPassword = record.Password.GetSaltedSha256(member.Salt);
                if (member.Password != saltedPassword) return string.Empty;
                
                var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, member.Account),
                                new Claim("UserName", member.MemberName),
                                new Claim("AvatarName", member.Avatar??""),
                                new Claim("MemberId", member.MemberId.ToString()),
                                new Claim(ClaimTypes.Email, member.Email),
                                new Claim(ClaimTypes.Role, "會員")
                            };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return "reload";
            }
            return string.Empty;
        }

        [HttpGet("logout")]
        public async Task<ActionResult<string>> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Url.Content("~/home/index");
        }

        ////載入縣市
        //public IActionResult Cities()
        //{
        //    var cities = _context.Address.Select(a => a.City).Distinct();
        //    return Json(cities);
        //}
        ////根據縣市載入鄉鎮區
        //public IActionResult Districts(string city)
        //{
        //    var district = _context.Address.Where(a => a.City == city)
        //        .Select(a => a.SiteId).Distinct();
        //    return Json(district);
        //}
    }
}
