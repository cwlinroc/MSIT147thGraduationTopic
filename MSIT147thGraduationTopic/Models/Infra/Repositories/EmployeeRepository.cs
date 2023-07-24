using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class EmployeeRepository
    {
        private readonly GraduationTopicContext _context;

        public EmployeeRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees()
        {
            var employees = _context.Employees.ToList();
            return employees.Select(o => o.ToDto());
        }

        public int CreateEmployee(EmployeeDto dto)
        {
            var obj = dto.ToEF();
            _context.Employees.Add(obj);
            _context.SaveChanges();
            return obj.EmployeeId;
        }

        public int EditEmployee(EmployeeEditDto dto, int employeeId, string fileName)
        {
            var employee = _context.Employees.FirstOrDefault(o => o.EmployeeId == employeeId);
            if (employee == null) return -1;

            employee.ChangeByEditDto(dto);
            if (!string.IsNullOrEmpty(fileName)) employee.AvatarName = fileName;

            _context.SaveChanges();
            return employeeId;
        }

        public int DeleteEmployee(int employeeId)
        {
            var employee = _context.Employees.Find(employeeId);
            if (employee == null) return -1;

            _context.Employees.Remove(employee);

            _context.SaveChanges();
            return employeeId;
        }


    }
}
