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
        private readonly IWebHostEnvironment _environemnt;
        //TODO add to app settings
        private readonly string[] _permissions = { "管理員", "經理", "員工" };
        public EmployeeService(GraduationTopicContext context, IWebHostEnvironment environemnt)
        {
            _context = context;
            _environemnt = environemnt;
            _repo = new EmployeeRepository(context);
        }

        public IEnumerable<EmployeeVM> GetAllEmployees()
        {
            return _repo.GetAllEmployees().Select(dto =>
            {
                string htmlFilePath = Path.Combine(_environemnt.WebRootPath, "uploads\\employeeAvatar");

                return dto.ToVM();
            });
        }
        public IEnumerable<EmployeeVM> GetEmployeesByNameOrAccount(string query)
        {
            return _repo.GetEmployeesByNameOrAccount(query).Select(dto =>
            {
                string htmlFilePath = Path.Combine(_environemnt.WebRootPath, "uploads\\employeeAvatar");

                return dto.ToVM();
            });
        }

        public int CreateEmployee(EmployeeDto dto, IFormFile? file)
        {
            if (file != null)
            {
                string path = Path.Combine(_environemnt.WebRootPath, @"uploads\employeeAvatar", file.FileName);

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
                string path = Path.Combine(_environemnt.WebRootPath, @"uploads\employeeAvatar", file.FileName);

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
            string s = _permissions[0];

            int permissionId = Array.IndexOf(_permissions, permission) + 1;

            if (permissionId <= 0 || permissionId > 3) return -1;

            return _repo.ChangeEmployeePermission(id, permissionId);
        }



        public int DeleteEmployee(int employeeId)
        {
            return _repo.DeleteEmployee(employeeId);
        }
    }
}
