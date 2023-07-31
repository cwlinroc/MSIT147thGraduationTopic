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

        [HttpPut("{id}")]
        public ActionResult<int> UpdateCoupon([FromForm] CouponEditDto cEDto, int id)
        {
            var couponId = _service.EditCoupon(cEDto, id);
            return couponId;
        }

        [HttpDelete("{id}")]
        public ActionResult<int> UpdateCoupon(int id)
        {
            return _service.DeleteCoupon(id);
        }
    }
}

