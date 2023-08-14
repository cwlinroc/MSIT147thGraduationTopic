using Microsoft.Extensions.Hosting;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.ExtendMethods;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.IO;
using System.Linq;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class EmployeeService
    {
        private readonly GraduationTopicContext _context;
        private readonly EmployeeRepository _repo;
        private readonly IWebHostEnvironment _environment;
        private readonly string[] _roles;
        public EmployeeService(GraduationTopicContext context
            , IWebHostEnvironment environment
            , string[] roles)
        {
            _context = context;
            _environment = environment;
            _repo = new EmployeeRepository(context);
            _roles = roles;
        }

        public IEnumerable<EmployeeVM> GetAllEmployees()
        {
            return _repo.GetAllEmployees().Select(dto =>
            {
                string htmlFilePath = Path.Combine(_environment.WebRootPath, "uploads\\employeeAvatar");

                return dto.ToVM();
            });
        }
        public IEnumerable<EmployeeVM> queryEmployeesByNameOrAccount(string query)
        {
            return _repo.queryEmployeesByNameOrAccount(query).Select(dto =>
            {
                string htmlFilePath = Path.Combine(_environment.WebRootPath, "uploads\\employeeAvatar");

                return dto.ToVM();
            });
        }

        public int CreateEmployee(EmployeeDto dto, IFormFile? file)
        {
            if (file != null)
            {
                string path = Path.Combine(_environment.WebRootPath, @"uploads\employeeAvatar", file.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                dto.EmployeeAvatarName = file.FileName;
            }

            var salt = new RandomGenerator().RandomSalt();
            dto.Salt = salt;
            dto.EmployeePassword = dto.EmployeePassword?.GetSaltedSha256(salt);
            dto.Permission = 3;

            return _repo.CreateEmployee(dto);
        }

        public int EditEmployee(EmployeeEditDto dto, int employeeId, IFormFile? file)
        {
            if (file != null)
            {
                string path = Path.Combine(_environment.WebRootPath, @"uploads\employeeAvatar", file.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            string? fileName = file?.FileName;
            return _repo.EditEmployee(dto, employeeId, fileName);
        }

        public int ChangeEmployeePermission(int id, string permission)
        {
            string s = _roles[0];

            int permissionId = Array.IndexOf(_roles, permission) + 1;

            if (permissionId <= 0 || permissionId > 3) return -1;

            return _repo.ChangeEmployeePermission(id, permissionId);
        }

        public int DeleteEmployee(int employeeId)
        {
            return _repo.DeleteEmployee(employeeId);
        }

        public async Task<bool> ConfirmWithPassword(int employeeId, string password)
        {
            (string? password, string? salt) employee = await _repo.GetPasswordAndSalt(employeeId);
            if (employee.password == null || employee.salt == null) return false;

            var saltedPassword = password.GetSaltedSha256(employee.salt);
            return saltedPassword == employee.password;
        }

        public async Task<string> GetAvatarName(int employeeId)
        {
            return await _repo.GetAvatarName(employeeId);
        }


        public async Task<int> UpdateAvatar(int employeeId, IFormFile? file)
        {
            string? fileName = string.Empty;
            if (file != null)
            {
                fileName = SaveFileToUpload(file);
            }
            return await _repo.UpdateAvatar(employeeId, fileName);
        }

        private string SaveFileToUpload(IFormFile file)
        {
            string path = Path.Combine(_environment.WebRootPath, @"uploads\employeeAvatar", file.FileName);
            using var fileStream = new FileStream(path, FileMode.Create);
            file.CopyTo(fileStream);
            return file.FileName;
        }

    }
}
