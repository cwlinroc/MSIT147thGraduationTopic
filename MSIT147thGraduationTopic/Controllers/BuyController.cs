using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;
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
        public IActionResult Index(params int[] ids)
        {
            string? json = HttpContext.Session.GetString("cartItemIds");
            if (!string.IsNullOrEmpty(json)) ids = JsonSerializer.Deserialize<int[]>(json);

            //TODO 刪掉

            if (!ids.Any()) ids = new int[] { 2, 3, 4 };

            (int id, string address, string phone) member = _service.GetMemberAddressAndPhone(ids[0]);

            ViewBag.MemberId = member.id;
            ViewBag.MemberAddress = member.address;
            ViewBag.MemberPhone = member.phone;

            //id驗證

            var items = _service.GetCartItems(ids);
            return View(items);
        }
    }
}
