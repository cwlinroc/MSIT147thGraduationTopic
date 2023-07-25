using MSIT147thGraduationTopic.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class EmployeeVM
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeAccount { get; set; }
        public string? Permission { get; set; }
        public string? EmployeeEmail { get; set; }
        public string? EmployeePhone { get; set; }
        public string? AvatarName { get; set; }
    }
    public class EmployeeCreateVM
    {
        [Required(AllowEmptyStrings = false)]
        public string? EmployeeName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? EmployeeAccount { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? EmployeePassword { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? EmployeeEmail { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? EmployeePhone { get; set; }
    }


    static public class EmployeeVMTransfer
    {
        static public EmployeeVM ToVM(this EmployeeDto dto)
        {
            string permission = dto.Permission switch
            {
                1 => "管理員",
                2 => "經理",
                3 => "員工",
                _ => "員工"
            };
            return new EmployeeVM
            {
                EmployeeId = dto.EmployeeId,
                EmployeeName = dto.EmployeeName,
                EmployeeAccount = dto.EmployeeAccount,
                EmployeeEmail = dto.EmployeeEmail,
                EmployeePhone = dto.EmployeePhone,
                Permission = permission,
                AvatarName = dto.EmployeeAvatarName,
            };
        }

        static public EmployeeDto ToDto(this EmployeeCreateVM vm)
        {
            return new EmployeeDto
            {
                EmployeeName = vm.EmployeeName,
                EmployeeAccount = vm.EmployeeAccount,
                EmployeePassword = vm.EmployeePassword,
                EmployeeEmail = vm.EmployeeEmail,
                EmployeePhone = vm.EmployeePhone
            };
        }
    }
}
