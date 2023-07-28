using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Runtime.CompilerServices;

namespace MSIT147thGraduationTopic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiCouponController
    {
        private readonly GraduationTopicContext _context;
        private readonly CouponService _service;
        private readonly IWebHostEnvironment _environment;

        public ApiCouponController(GraduationTopicContext context, CouponService service, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _service = new CouponService(context, environment);
        }

        [HttpGet]
        public ActionResult<List<CouponVM>> GetAllCoupons()
        {
            return _service.GetAllCoupons().ToList();
        }

        [HttpPost]
        public ActionResult<int> CreateCoupon([FromForm] CouponCreateVM vm)
        {
            var couponId = _service.CreateCoupon(vm.ToDto());
            return couponId;
        }

        [HttpPut("{id}")]
        public ActionResult<int> UpdateCoupon([FromForm] CouponEditDto cEDto,int id)
        {
            var couponId = _service.EditCoupon(cEDto,id);
            return couponId;
        }

        [HttpDelete("{id}")]
        public ActionResult<int>UpdateCoupon(int id)
        {
            return _service.DeleteCoupon(id);
        }
    }
}
