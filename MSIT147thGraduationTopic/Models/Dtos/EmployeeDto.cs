using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = null!;

        public string EmployeeAccount { get; set; } = null!;

        public string EmployeePassword { get; set; } = null!;

        public int Permission { get; set; }

        public string EmployeeEmail { get; set; } = null!;

        public string EmployeePhone { get; set; } = null!;

        public string? EmployeeAvatarPath { get; set; }
    }

    static public class EmployeeTransfer
    {
        static public Employee ToEF(this EmployeeDto dto)
        {
            return new Employee
            {
                EmployeeAccount = dto.EmployeeAccount,
                EmployeeAvatarPath = dto.EmployeeAvatarPath,
                EmployeeEmail = dto.EmployeeEmail,
                EmployeeId = dto.EmployeeId,
                EmployeeName = dto.EmployeeName,
                EmployeePassword = dto.EmployeePassword,
                Permission = dto.Permission,
                EmployeePhone = dto.EmployeePhone,
            };
        }
    }
}
