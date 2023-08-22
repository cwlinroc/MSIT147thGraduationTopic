using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Policy;

namespace MSIT147thGraduationTopic.Models.Services
{
    public record LoginRecord([Required] string Account, [Required] string Password);
    public interface IAuthService
    {
        Task<string> AuthenticateUser(LoginRecord record);
    }

    public class AuthService : IAuthService
    {
        private readonly GraduationTopicContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] _employeeRoles;
        private readonly IUrlHelper _url;

        public AuthService(GraduationTopicContext context, IHttpContextAccessor httpContextAccessor
            , IUrlHelper url)
        {
            _context = context;
            _url = url;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> AuthenticateUser(LoginRecord record)
        {
            var emp = await _context.Employees
                      .FirstOrDefaultAsync(o => o.EmployeeAccount == record.Account);
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
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                    , new ClaimsPrincipal(claimsIdentity));
                return _url.Content("~/employeebackstage/welcome");
            }
            else if (member != null && member.IsActivated)
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
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                    , new ClaimsPrincipal(claimsIdentity));                
                return "reload";
            }
            else if (member != null && !member.IsActivated)
            {
                return "Member/NoRole";
            }

            return string.Empty;
        }
    }
}
