using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;
using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;

namespace MSIT147thGraduationTopic.Controllers
{
    public class BuyController : Controller
    {

        private readonly GraduationTopicContext _context;
        private readonly BuyServices _service;

        public BuyController(GraduationTopicContext context)
        {
            _context = context;
            _service = new BuyServices(context);
        }

        [HttpGet]
        public async Task<IActionResult> Index(params int[] ids)
        {
            string? json = HttpContext.Session.GetString("cartItemIds");
            if (!string.IsNullOrEmpty(json)) ids = JsonSerializer.Deserialize<int[]>(json)!;

            if (!ids.Any()) return RedirectToAction("Index", "Cart");

            var member = _service.GetMemberAddressAndPhone(ids[0]);
            if (member == null) return BadRequest();

            //TODO-cw id驗證
            ViewBag.MemberId = member.MemberId;
            ViewBag.MemberAddress = member.Address;
            ViewBag.MemberPhone = member.Phone;
            ViewBag.MemberName = member.MemberName;
            ViewBag.MemberEmail = member.Email;

            //var cartItemsTask = _service.GetCartItems(ids);
            //var couponsTask = _service.GetAllCouponsAvalible(member.MemberId);
            //await Task.WhenAll(cartItemsTask, couponsTask);
            //ViewBag.Coupons = couponsTask.Result;
            //return View(cartItemsTask.Result);

            var cartItems = await _service.GetCartItems(ids);
            var coupons = await _service.GetAllCouponsAvalible(member.MemberId);
            ViewBag.Coupons = coupons;
            return View(cartItems);
        }



        public record OrderRecord([Required] string Address,
            [Required] string Phone,
            string? CouponId,
            [Required] string Payment,
            string Remark);

        [HttpPost]
        public IActionResult Index(OrderRecord record)
        {
            //memberId
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
            {
                return BadRequest("找不到對應會員ID");
            }

            //cartItemIds
            string? json = HttpContext.Session.GetString("cartItemIds");
            if (string.IsNullOrEmpty(json)) return BadRequest("沒有預計購買的商品");
            int[] cartItemIds = JsonSerializer.Deserialize<int[]>(json)!;


            int result = _service.CreateOrder(cartItemIds, memberId, record);
            if (result < 0) return BadRequest();


            //??
            return RedirectToAction("Succeed");
        }


        public IActionResult Succeed()
        {
            return View();
        }
    }
}
