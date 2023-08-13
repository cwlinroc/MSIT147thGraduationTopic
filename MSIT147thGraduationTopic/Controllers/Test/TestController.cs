using Microsoft.AspNetCore.Mvc;

namespace MSIT147thGraduationTopic.Controllers.Test
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.BaseUrl = $"{Request.Scheme}://{Request.Host}";
            return View();
        }

        public IActionResult Confirm(long transactionId, string orderId)
        {
            ViewBag.BaseUrl = $"{Request.Scheme}://{Request.Host}";

            ViewBag.TransactionId = transactionId ;
            ViewBag.OrderId = orderId ;
            return View();
        }
        public IActionResult Cancel()
        {
            return View();
        }

    }
}
