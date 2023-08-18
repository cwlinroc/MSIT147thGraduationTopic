using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Security.Claims;

namespace MSIT147thGraduationTopic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCouponController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly CouponService _service;
        private readonly IWebHostEnvironment _environment;

        public ApiCouponController(GraduationTopicContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _service = new CouponService(context, environment);
        }

        [HttpPost]
        public ActionResult<int> CreateCoupon([FromForm] CouponCreateVM vm)
        {
            var couponId = _service.CreateCoupon(vm.ToDto());
            return couponId;
        }

        [HttpGet("{id}")]
        public ActionResult<CouponVM> GetCouponById(int id)
        {
            var couponData = _service.GetCouponById(id);
            if (couponData == null)
            {
                return NotFound();
            }
            return couponData.ToVM();
        }

        public record ReceiveCouponRecord(int id);

        [HttpPost("couponreceive")]
        public ActionResult<int> CouponReceive(ReceiveCouponRecord record)
        {
            //memberId
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
            {
                return BadRequest("找不到對應會員ID");
            }
            var dataRowChanged = _service.CouponReceive(memberId,record.id);
            return dataRowChanged;
        } 

        [HttpPut("{id}")]
        public ActionResult<int> EditCoupon(int id, [FromForm] CouponEditDto cEDto)
        {
            if(cEDto == null)
            {
                return BadRequest("查無資料");
            }
            if(id!=cEDto.CouponId)
            {
                return BadRequest("id序號不相符");
            }
            var couponData = _service.GetCouponById(id);
            if(couponData == null)
            {
                return NotFound("查無資料");
            }
            int updatedCouponId = _service.EditCoupon(cEDto);
            if(updatedCouponId == -1)
            {
                return NotFound("查無資料");
            }
            var updatedCoupon = _service.GetCouponById(updatedCouponId);
            return Ok(updatedCoupon);
        }

        [HttpDelete("{id}")]
        public ActionResult<int> UpdateCoupon(int id)
        {
            return _service.DeleteCoupon(id);
        }
    }
}

