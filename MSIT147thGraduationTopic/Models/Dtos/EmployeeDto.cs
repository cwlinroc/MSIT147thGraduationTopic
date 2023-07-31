using MSIT147thGraduationTopic.EFModels;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.Dtos
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeAccount { get; set; }
        public string? EmployeePassword { get; set; }
        public string? Salt { get; set; }
        public int Permission { get; set; }
        public string? EmployeeEmail { get; set; }
        public string? EmployeePhone { get; set; }
        public string? EmployeeAvatarName { get; set; }
    }

    public class EmployeeEditDto
    {
        [Required(AllowEmptyStrings = false)]
        public string? EmployeeName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? EmployeeEmail { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? EmployeePhone { get; set; }
    }

    static public class EmployeeTransfer
    {
        static public Employee ToEF(this EmployeeDto dto)
        {
            return new Employee
            {
                EmployeeAccount = dto.EmployeeAccount,
                AvatarName = dto.EmployeeAvatarName,
                EmployeeEmail = dto.EmployeeEmail,
                EmployeeId = dto.EmployeeId,
                EmployeeName = dto.EmployeeName,
                EmployeePassword = dto.EmployeePassword,
                Permission = dto.Permission,
                EmployeePhone = dto.EmployeePhone,
                Salt = dto.Salt,
            };
        }

        static public EmployeeDto ToDto(this Employee entity)
        {
            return new EmployeeDto
            {
                EmployeeAccount = entity.EmployeeAccount,
                EmployeeAvatarName = entity.AvatarName,
                EmployeeEmail = entity.EmployeeEmail,
                EmployeeId = entity.EmployeeId,
                EmployeeName = entity.EmployeeName,
                EmployeePassword = entity.EmployeePassword,
                Permission = entity.Permission,
                EmployeePhone = entity.EmployeePhone,
                //Salt = entity.Salt,
            };
        }

        static public void ChangeByEditDto(this Employee entity, EmployeeEditDto dto)
        {
            entity.EmployeeName = dto.EmployeeName;
            entity.EmployeePhone = dto.EmployeePhone;
            entity.EmployeeEmail = dto.EmployeeEmail;
        }


    }
}
