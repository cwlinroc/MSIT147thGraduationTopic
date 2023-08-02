using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;

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

        //public ApiCouponController()
        //{

        //}

        //[HttpPost]
        //public ActionResult<int> CreateCoupon([FromForm] CouponCreateVM vm)
        //{
        //    return -1;
        //}

        //[HttpGet]
        //public ActionResult<List<CouponVM>> GetAllCoupons()
        //{
        //    return _service.GetAllCoupons().ToList();
        //}

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

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult<int> EditCoupon(int id, [FromBody] CouponEditDto cEDto)
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

