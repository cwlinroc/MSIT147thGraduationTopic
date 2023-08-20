using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Paypal;

namespace MSIT147thGraduationTopic.Controllers.Paypal
{
    public class PaypalController : Controller
    {
        private readonly PaypalClient _paypalClient;
        private readonly GraduationTopicContext _context;
        public PaypalController(PaypalClient paypalClient, GraduationTopicContext context)
        {
            this._paypalClient = paypalClient;
            _context = context;
        }

        public IActionResult Index()
        {
            // ViewBag.ClientId is used to get the Paypal Checkout javascript SDK
            ViewBag.ClientId = _paypalClient.ClientId;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                // set the transaction price and currency
                var price = _context.Orders.FirstOrDefault(o => o.OrderId == orderId)?.PaymentAmount.ToString();
                price ??= "1689";
                var currency = "TWD";

                // "reference" is the transaction key
                var reference = orderId.ToString();

                var response = await _paypalClient.CreateOrder(price, currency, reference);

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Order(CancellationToken cancellationToken)
        {
            try
            {
                // set the transaction price and currency
                var price = "100.00";
                var currency = "TWD";

                // "reference" is the transaction key
                var reference = "INV001";

                var response = await _paypalClient.CreateOrder(price, currency, reference);

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }

        public async Task<IActionResult> Capture(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);

                var reference = response.purchase_units[0].reference_id;

                // Put your logic to save the transaction here
                // You can use the "reference" variable as a transaction key

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
