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
    public class ApiMemberController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly MemberService _service;
        private readonly IWebHostEnvironment _environment;

        public ApiMemberController(GraduationTopicContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _service = new MemberService(context, environment);
        }

        [HttpGet]
        public ActionResult<List<MemberVM>> GetAllMembers()
        {
            return _service.GetAllMembers().ToList();
        }

        [HttpPost]
        public ActionResult<int> CreateMember([FromForm] MemberCreateVM vm, [FromForm] IFormFile avatar)
        {
            var employeeId = _service.CreateMember(vm.ToDto(), avatar);

            return employeeId;
        }

        [HttpPut("{id}")]
        public ActionResult<int> UpdateMember([FromForm] MemberEditDto dto, int id, [FromForm] IFormFile avatar)
        {
            var memberId = _service.EditMember(dto, id, avatar);

            return memberId;
        }

        [HttpDelete("{id}")]
        public ActionResult<int> UpdateMember(int id)
        {
            return _service.DeleteMember(id);
        }

        ////載入縣市
        //public IActionResult Cities()
        //{
        //    var cities = _context.Address.Select(a => a.City).Distinct();
        //    return Json(cities);
        //}
        ////根據縣市載入鄉鎮區
        //public IActionResult Districts(string city)
        //{
        //    var district = _context.Address.Where(a => a.City == city)
        //        .Select(a => a.SiteId).Distinct();
        //    return Json(district);
        //}
    }
}
