﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Dtos.LinePay;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.LinePay;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using NuGet.Packaging.Signing;

namespace MSIT147thGraduationTopic.Controllers.Buy
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBuyController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly BuyServices _service;

        public ApiBuyController(GraduationTopicContext context)
        {
            _context = context;
            _service = new BuyServices(context);
        }

        [HttpGet("coupons")]
        public async Task<IEnumerable<BuyPageCouponVM>> GetAllCouponsAvalible()
        {
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
            {
                return new List<BuyPageCouponVM>();
            }
            var item = await _service.GetAllCouponsAvalible(memberId);

            return item;
        }

        [HttpGet("cartitems")]
        [HttpGet("cartitems/{couponId}")]
        public async Task<BuyPageCartItemsListVM?> GetCalculatedCartItems(int? couponId)
        {
            string? json = HttpContext.Session.GetString("cartItemIds");
            int[] ids = JsonSerializer.Deserialize<int[]>(json ?? "[]")!;

            return await _service.GetCartItemsWithCoupons(ids, couponId);
        }



        public record OrderRecord(
            [Required] string City,
            [Required] string District,
            [Required] string Address,
            [Required] string Phone,
            string? CouponId,
            [Required] string Payment,
            string? Remark);

        [HttpPost("sendorder")]
        public async Task<ActionResult<dynamic>> SendOrder([FromForm] OrderRecord record)
        {
            //memberId
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
            {
                return BadRequest("找不到對應會員ID");
            }
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            //cartItemIds
            string? json = HttpContext.Session.GetString("cartItemIds");
            if (string.IsNullOrEmpty(json)) return BadRequest("沒有預計購買的商品");
            int[] cartItemIds = JsonSerializer.Deserialize<int[]>(json)!;

            //create order
            (int orderId, int totalPaymentAmount, var checkoutDtos) = await _service.CreateOrder(cartItemIds, memberId, record);

            //linepayresponse
            if (record.Payment == "2")
            {
                var linepayservice = new LinePayService();
                var linepayRequestDto = linepayservice.GetPaymentRequestDto(orderId, totalPaymentAmount, baseUrl, checkoutDtos);
                var linepayResponseDto = await linepayservice.SendPaymentRequest(linepayRequestDto);
                //return new JsonResult(linepayResponseDto);
                return linepayResponseDto;
            }

            //??
            return null;

        }



    }
}
