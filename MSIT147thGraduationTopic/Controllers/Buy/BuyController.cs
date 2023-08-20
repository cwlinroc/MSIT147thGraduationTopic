using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Paypal;
using MSIT147thGraduationTopic.Models.Services;
using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using System.Text.Json;

namespace MSIT147thGraduationTopic.Controllers.Buy
{
    public class BuyController : Controller
    {

        private readonly GraduationTopicContext _context;
        private readonly BuyServices _service;
        private readonly PaypalClient _paypalClient;

        public BuyController(GraduationTopicContext context, PaypalClient paypalClient)
        {
            _context = context;
            _service = new BuyServices(context);
            _paypalClient = paypalClient;
        }

        [HttpGet]
        [Authorize(Roles = "會員")]
        public async Task<IActionResult> Index(params int[] ids)
        {
            string? json = HttpContext.Session.GetString("cartItemIds");
            if (!string.IsNullOrEmpty(json)) ids = JsonSerializer.Deserialize<int[]>(json)!;

            if (!ids.Any()) return RedirectToAction("Index", "Cart");

            var member = _service.GetMemberData(ids[0]);
            if (member == null) return BadRequest();

            //TODO-cw id驗證
            ViewBag.MemberId = member.MemberId;
            ViewBag.MemberCity = member.City;
            ViewBag.MemberDistrict = member.District;
            ViewBag.MemberAddress = member.Address;
            ViewBag.MemberPhone = member.Phone;
            ViewBag.MemberName = member.MemberName;
            ViewBag.MemberEmail = member.Email;

            var cartItems = await _service.GetCartItems(ids);
            var coupons = await _service.GetAllCouponsAvalible(member.MemberId);
            ViewBag.Coupons = coupons;
            return View(cartItems);
        }

        [Authorize(Roles = "會員")]
        public IActionResult PaypalPayment(int orderId)
        {
            ViewBag.ClientId = _paypalClient.ClientId;
            ViewBag.OrderId = orderId;

            return View();
        }

        [Authorize(Roles = "會員")]
        public IActionResult PaypalSuccess()
        {
            return View();
        }

        [Authorize(Roles = "會員")]
        public IActionResult LinePayConfirm(long transactionId, string orderId)
        {
            ViewBag.BaseUrl = $"{Request.Scheme}://{Request.Host}";
            ViewBag.TransactionId = transactionId;
            ViewBag.OrderId = orderId;
            ViewBag.Payment = _context.Orders.Find(int.Parse(orderId))?.PaymentAmount;

            return View();
        }

        [Authorize(Roles = "會員")]
        public IActionResult Succeed()
        {
            return View();
        }


        //TODO-cw 失敗頁面
        [Authorize(Roles = "會員")]
        public IActionResult Failed()
        {
            return View();
        }
    }
}
