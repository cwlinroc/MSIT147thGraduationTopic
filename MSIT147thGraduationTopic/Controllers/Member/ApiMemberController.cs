using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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
using static MSIT147thGraduationTopic.Models.Infra.Utility.MailSetting;
using System.Security.Policy;
using MSIT147thGraduationTopic.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MSIT147thGraduationTopic.Controllers.Member
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiMemberController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly MemberService _service;
        private readonly ShoppingHistoryService _shService;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _environment;
        private readonly IUrlHelper _url;

        private readonly string[] _employeeRoles;

        public ApiMemberController(GraduationTopicContext context, IMailService mailService
            , IWebHostEnvironment environment, IOptions<OptionSettings> options, IUrlHelper url)
        {
            _context = context;
            _environment = environment;
            _mailService = mailService;
            _url = url;
            _service = new MemberService(context, environment);
            _shService = new ShoppingHistoryService(context, environment);

            _employeeRoles = options.Value.EmployeeRoles!;
        }

        [HttpGet]
        public ActionResult<List<MemberVM>> GetAllMembers()
        {
            return _service.GetAllMembers().ToList();
        }

        [HttpGet("self")]
        public ActionResult<MemberVM> GetMember()
        {
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
            {
                return BadRequest("找不到對應會員ID");
            }

            return _service.GetMember(memberId).ToVM();
        }

        [HttpGet("{query}")]
        public ActionResult<List<MemberVM>> GetMemberByNameOrAccount(string query)
        {
            return _service.GetMemberByNameOrAccount(query).ToList();
        }


        [HttpGet("ShoppingHistory")]
        public ActionResult<List<ShoppingHistoryDto>> GetOrdersByMemberId()
        {
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
            {
                return BadRequest("找不到對應會員ID");
            }

            var list = _shService.GetOrdersByMemberId(memberId).ToList();
            return list;
        }

        [HttpPost]
        public ActionResult<int> CreateMember([FromForm] MemberCreateVM vm, [FromForm] IFormFile? avatar)
        {
            try
            {
                if (_context.Members.Any(m => m.Account == vm.Account))
                    return Content("帳號已存在");

                var memberId = _service.CreateMember(vm.ToDto(), avatar);
                string body = _mailService.CreateUrl(vm.Account, _url, "EmailVerify", "Member");

                MailRequest request = new MailRequest()
                {
                    ToEmail = vm.Email,
                    Subject = "福祿獸購物商城帳號驗證信",
                    Body = $"<html><body><h1>驗證確認</h1><h3><a href=\"{body}\">請點這裡驗證</a></h3></body></html>"
                };

                _mailService.SendEmailAsync(request);

                return memberId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("accountexist")]
        public ActionResult<bool> AccountExist(string account)
        {
            if (_context.Members.Any(m => m.Account == account)
                || _context.Employees.Any(m => m.EmployeeAccount == account))
            {
                return true;
            }
            return false;
        }

        [HttpPut("{id}")]
        public ActionResult<int> UpdateMember([FromForm] MemberEditDto dto, int id, [FromForm] IFormFile? avatar)
        {
            try
            {
                var memberId = _service.EditMember(dto, id, avatar);
                return memberId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("memberCenter")]
        public ActionResult<int> UpdateSelfData([FromForm] MemberCenterEditVM vm, [FromForm] IFormFile? avatar)
        {
            try
            {
                int id = int.Parse(HttpContext.User.FindFirstValue("MemberId"));
                var memberId = _service.EditMember(vm.ToCenterEditDto(), id, avatar);
                return memberId;
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpDelete("{id}")]
        public ActionResult<int> UpdateMember(int id)
        {
            return _service.DeleteMember(id);
        }

        [HttpGet("selfavatar")]
        [Authorize(Roles = "會員")]
        public async Task<ActionResult<string>> GetSelfAvatar()
        {
            try
            {
                if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
                {
                    return BadRequest("找不到對應ID");
                }
                return await _service.GetAvatarName(memberId);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public record LoginRecord([Required] string Account, [Required] string Password);
        [HttpPost("login")]
        public async Task<ActionResult<string>> LogIn(LoginRecord record)
        {
            try
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
                                new Claim("EmployeeId", emp.EmployeeId.ToString()),
                                new Claim(ClaimTypes.Email, emp.EmployeeEmail),
                                new Claim(ClaimTypes.Role, _employeeRoles[emp.Permission-1])
                            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                        , new ClaimsPrincipal(claimsIdentity));

                    return Url.Content("~/employeebackstage/welcome");
                }

                var member = await _context.Members.Select(o => new
                {
                    o.Account,
                    o.Password,
                    o.Salt,
                    o.MemberName,
                    o.NickName,
                    o.Email,
                    o.Avatar,
                    o.MemberId,
                    o.IsActivated
                }).FirstOrDefaultAsync(o => o.Account == record.Account);

                if (member != null && member.IsActivated)
                {
                    string saltedPassword = record.Password.GetSaltedSha256(member.Salt);
                    if (member.Password != saltedPassword) return string.Empty;

                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, member.Account),
                                new Claim("UserName", member.MemberName),
                                new Claim("NickName", member.NickName??""),
                                new Claim("AvatarName", member.Avatar??""),
                                new Claim("MemberId", member.MemberId.ToString()),
                                new Claim(ClaimTypes.Email, member.Email),
                                new Claim(ClaimTypes.Role, "會員")
                            };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                        , new ClaimsPrincipal(claimsIdentity));
                    HttpContext.Session.SetString("LoadCoupon", "Load");
                    return "reload";
                }
                else if (member != null && !member.IsActivated)
                {
                    return "Member/NoRole";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("logout")]
        public async Task<ActionResult<string>> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Url.Content("~/home/index");
        }

        public record GoogleLoginRecord([Required] string Email);
        [HttpPost("googlelogin")]
        public async Task<ActionResult<string>> GoogleLogIn(GoogleLoginRecord record)
        {
            try
            {
                var emp = await _context.Employees
                          .FirstOrDefaultAsync(o => o.EmployeeEmail == record.Email);

                if (emp != null)
                {
                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, emp.EmployeeAccount),
                                new Claim("UserName", emp.EmployeeName),
                                new Claim("AvatarName", emp.AvatarName??""),
                                new Claim("EmployeeId", emp.EmployeeId.ToString()),
                                new Claim(ClaimTypes.Email, emp.EmployeeEmail),
                                new Claim(ClaimTypes.Role, _employeeRoles[emp.Permission-1])
                            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                        , new ClaimsPrincipal(claimsIdentity));

                    return Url.Content("~/employeebackstage/welcome");
                }

                var member = await _context.Members.Select(o => new
                {
                    o.Account,
                    o.Password,
                    o.Salt,
                    o.MemberName,
                    o.NickName,
                    o.Email,
                    o.Avatar,
                    o.MemberId,
                    o.IsActivated
                }).FirstOrDefaultAsync(o => o.Email == record.Email);

                if (member != null && member.IsActivated)
                {
                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, member.Account),
                                new Claim("UserName", member.MemberName),
                                new Claim("NickName", member.NickName??""),
                                new Claim("AvatarName", member.Avatar??""),
                                new Claim("MemberId", member.MemberId.ToString()),
                                new Claim(ClaimTypes.Email, member.Email),
                                new Claim(ClaimTypes.Role, "會員")
                            };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                        , new ClaimsPrincipal(claimsIdentity));
                    HttpContext.Session.SetString("LoadCoupon", "Load");
                    return "reload";
                }
                else if (member != null && !member.IsActivated)
                {
                    return "Member/NoRole";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
