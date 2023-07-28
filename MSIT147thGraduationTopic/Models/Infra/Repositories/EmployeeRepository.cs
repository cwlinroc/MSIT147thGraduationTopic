using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.EFModels;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<EmployeeDto> queryEmployeesByNameOrAccount(string query)
        {
            var employees = _context.Employees
                .Where(o => o.EmployeeName.Contains(query) || o.EmployeeAccount.Contains(query));
            return employees.Select(o => o.ToDto());
        }

        public async Task<EmployeeDto?> GetEmployeeByAccount(string account)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(o => o.EmployeeAccount == account);
            return employee?.ToDto();
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

        public int ChangeEmployeePermission(int id, int permissionId)
        {
            var employee = _context.Employees.FirstOrDefault(o => o.EmployeeId == id);
            if (employee == null) return -1;

            employee.Permission = permissionId;
            _context.SaveChanges();
            return id;
        }

        public int DeleteEmployee(int employeeId)
        {
            var employee = _context.Employees.Find(employeeId);
            if (employee == null) return -1;

            _context.Employees.Remove(employee);

            _context.SaveChanges();
            return employeeId;
        }

        public (string?, string?) GetPasswordAndSalt(int memberId)
        {
            var member = _context.Members.Where(o => o.MemberId == memberId)
                .Select(o => new { o.Password, o.Salt })
                .FirstOrDefault();
            return (member?.Password, member?.Salt);
        }
    }
}
