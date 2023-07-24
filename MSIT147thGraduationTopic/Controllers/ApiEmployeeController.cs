using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpGet("{query}")]
        public ActionResult<List<EmployeeVM>> GetEmployeesByNameOrAccount(string query)
        {
            return _service.GetEmployeesByNameOrAccount(query).ToList();
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

        //[HttpPut("permission/{id}")]
        //public ActionResult<int> UpdateEmployeePermission(int id, string permission)
        //{
        //    var employeeId = _service.EditEmployee(dto, id, avatar);

        //    return employeeId;
        //}

        [HttpDelete("{id}")]
        public ActionResult<int> UpdateEmployee(int id)
        {
            return _service.DeleteEmployee(id);
        }
    }
}
