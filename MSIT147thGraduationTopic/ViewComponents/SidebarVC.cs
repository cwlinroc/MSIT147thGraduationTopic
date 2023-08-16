using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Security.Claims;

namespace MSIT147thGraduationTopic.ViewComponents
{
    public class SidebarVC : ViewComponent
    {
        private readonly GraduationTopicContext _context;
        private readonly MemberService _service;
        private readonly IWebHostEnvironment _environment;

        public SidebarVC(GraduationTopicContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _service = new MemberService(context, environment);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int? memberId = int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int temp) ? temp : null;
            string avatarName = "memberDefault.png";
            if (memberId != null)
            {
                var memberAvatar = await _service.GetAvatarName(memberId.Value);
                if (!memberAvatar.IsNullOrEmpty()) avatarName = memberAvatar;
            }
            return View("Default", avatarName);
        }
    }
}
