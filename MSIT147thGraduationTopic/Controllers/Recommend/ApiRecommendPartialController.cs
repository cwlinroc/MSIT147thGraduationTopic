using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;
using MSIT147thGraduationTopic.Models.Services;
using System.Data;
using System.Security.Claims;

namespace MSIT147thGraduationTopic.Controllers.Recommend
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiRecommendPartialController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly RecommendPartialService _service;

        public ApiRecommendPartialController(GraduationTopicContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _service = new(context, accessor);
        }


        [HttpGet("favorspecs/{merchandiseId}")]
        public async Task<ActionResult<List<SpecDisplyDto>>> GetFavorSpecs(int? merchandiseId)
        {
            return await _service.GetFavorSpecs(merchandiseId);
        }


        [HttpGet("popularspecs/{merchandiseId}")]
        public async Task<ActionResult<List<SpecDisplyDto>>> GetPopularSpecs(int? merchandiseId)
        {
            return await _service.GetPopularSpecs(merchandiseId);
        }


        [HttpGet("addincart/{specId}")]
        [Authorize(Roles = "會員")]
        public async Task<ActionResult<int>> AddInCart(int specId)
        {
            int memberId = int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int tempInt) ? tempInt : -1;
            if (memberId < 0) return -1;

            var cartitem = await _context.CartItems.FirstOrDefaultAsync(o => o.MemberId == memberId && o.SpecId == specId);

            if (cartitem != null)
            {
                cartitem.Quantity++;
                return await _context.SaveChangesAsync();
            }

            cartitem = new CartItem { SpecId = specId, MemberId = memberId, Quantity = 1 };
            _context.CartItems.Add(cartitem);
            return await _context.SaveChangesAsync();
        }
    }
}
