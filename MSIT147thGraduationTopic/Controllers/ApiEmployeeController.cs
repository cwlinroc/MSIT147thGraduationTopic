using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Services;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MSIT147thGraduationTopic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEmployeeController : ControllerBase
    {
        private readonly GraduationTopicContext _context;
        private readonly EmployeeService _service;
        private readonly IWebHostEnvironment _environment;

        public ApiEmployeeController(GraduationTopicContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _service = new EmployeeService(context, environment);
        }

        [HttpGet]
        public ActionResult<List<EmployeeVM>> GetAllEmployees()
        {
            return _service.GetAllEmployees().ToList();
        }

        [HttpGet("{query}")]
        public ActionResult<List<EmployeeVM>> GetEmployeesByNameOrAccount(string query)
        {
            return _service.queryEmployeesByNameOrAccount(query).ToList();
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

        public record Container([Required] string Permission);

        [HttpPut("permission/{id}")]
        public ActionResult<int> UpdateEmployeePermission(Container permission, int id = 0)
        {
            var employeeId = _service.ChangeEmployeePermission(id, permission.Permission);

            return employeeId;
        }


        [HttpDelete("{id}")]
        public ActionResult<int> UpdateEmployee(int id)
        {
            return _service.DeleteEmployee(id);
        }
    }
}
