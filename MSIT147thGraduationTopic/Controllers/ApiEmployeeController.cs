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
    public class ApiEmployeeController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly EmployeeService _service;
        private readonly IWebHostEnvironment _environemnt;

        public ApiEmployeeController(GraduationTopicContext context, IWebHostEnvironment environemnt)
        {
            _context = context;
            _environemnt = environemnt;
            _service = new EmployeeService(context, environemnt);
        }


        [HttpGet]
        public ActionResult<List<EmployeeVM>> GetAllEmployees()
        {
            return _service.GetAllEmployees().ToList();
        }

        [HttpPost]
        public ActionResult<int> CreateEmployee([FromForm] EmployeeCreateVM vm, [FromForm] IFormFile? avatar)
        {
            var employeeId = _service.CreateEmployee(vm.ToDto(), avatar);

            return employeeId;
        }

        [HttpPut("{id}")]
        public ActionResult<int> UpdateEmployee([FromForm] EmployeeEditDto dto, int id, [FromForm] IFormFile? avatar)
        {
            var employeeId = _service.EditEmployee(dto, id, avatar);

            return employeeId;
        }

        [HttpDelete("{id}")]
        public ActionResult<int> UpdateEmployee(int id)
        {
            return _service.DeleteEmployee(id);
        }
    }
}
