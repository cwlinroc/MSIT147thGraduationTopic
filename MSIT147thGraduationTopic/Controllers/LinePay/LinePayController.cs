using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.LinePay;
using MSIT147thGraduationTopic.Models.LinePay;
using MSIT147thGraduationTopic.Models.Services;
using System.Data;

namespace MSIT147thGraduationTopic.Controllers.LinePay
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinePayController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly LinePayService _linePayService;
        public LinePayController(GraduationTopicContext context)
        {
            _linePayService = new LinePayService();
            _context = context;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "會員")]
        public async Task<PaymentResponseDto> CreatePayment(PaymentRequestDto dto)
        {
            return await _linePayService.SendPaymentRequest(dto);
        }

        [HttpPost("Confirm")]
        [Authorize(Roles = "會員")]
        public async Task<PaymentConfirmResponseDto> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId, PaymentConfirmDto dto)
        {
            var response = await _linePayService.ConfirmPayment(transactionId, orderId, dto);

            await new BuyServices(_context).ConfirmOrder(int.Parse(orderId));

            return response;
        }

        [HttpGet("Cancel")]
        [Authorize(Roles = "會員")]
        public async void CancelTransaction([FromQuery] string transactionId)
        {
            _linePayService.TransactionCancel(transactionId);
        }
    }
}
