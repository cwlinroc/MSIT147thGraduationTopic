using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Security.Claims;

namespace MSIT147thGraduationTopic.Controllers.Cart
{
    public class CartController : Controller
    {
        [Authorize(Roles = "會員")]
        public IActionResult Index()
        {
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
            {
                return BadRequest("找不到對應會員ID");
            }

            return View(memberId);
        }

    }
}
