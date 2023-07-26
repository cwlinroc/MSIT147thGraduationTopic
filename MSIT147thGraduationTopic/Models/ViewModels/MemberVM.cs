using MSIT147thGraduationTopic.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class MemberVM
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string NickName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string Account { get; set; }        
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }        
        public bool IsActivated { get; set; }
        public string ConfirmGuid { get; set; }
    }

    public class MemberCreateVM
    {
        [Required(AllowEmptyStrings = false)]
        public string? MemberName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? Account { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? Password { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? Phone { get; set; }
    }

    public static partial class MemberVMTransfer
    {
        public static MemberVM ToVM(this MemberDto dto)
        {
            return new MemberVM
            {
                MemberId = dto.MemberId,
                MemberName = dto.MemberName,
                NickName = dto.NickName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Account = dto.Account,                
                Phone = dto.Phone,
                Address = dto.Address,
                Email = dto.Email
            };
        }

        public static MemberDto ToDto(this MemberCreateVM vm)
        {
            return new MemberDto
            {
                
                MemberName = vm.MemberName,                
                Account = vm.Account,
                Password = vm.Password,
                Phone = vm.Phone,               
                Email = vm.Email
            };
        }
    }
}
