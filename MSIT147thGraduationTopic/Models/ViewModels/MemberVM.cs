using Humanizer;
using MSIT147thGraduationTopic.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class MemberVM
    {
        public int MemberId { get; set; }
        public string? MemberName { get; set; }
        public string? NickName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public string? Salt { get; set; }
        public bool IsActivated { get; set; }
        public string? ConfirmGuid { get; set; }
    }

    public class MemberCreateVM
    {
        public int MemberId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? MemberName { get; set; }

        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? NickName { get; set; }

        [Required]
        [DateTimeRange(-100, -18, ErrorMessage = "年齡不可大於100歲,小於18歲!")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string? Gender { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(15, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Account { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string? Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(20, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Phone { get; set; }

        public string? City { get; set; }
        public string? District { get; set; }

        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Address { get; set; }

        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Email { get; set; }
        public string? Avatar { get; set; }

    }

    public class MemberEditVM
    {
        public int MemberId { get; set; }
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? NickName { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }

        [MaxLength(20, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }

        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Address { get; set; }

        [EmailAddress]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Email { get; set; }
        public string? Avatar { get; set; }
    }

    public class MemberCenterEditVM
    {
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? NickName { get; set; }
        public string? Password { get; set; }

        [MaxLength(20, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }

        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Address { get; set; }

        [EmailAddress]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public bool IsActivated { get; set; }
    }

    public class MemberSearchVM
    {
        [Display(Name = "編號")]
        public int MemberID { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? MemberName { get; set; }

        [Display(Name = "暱稱")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? NickName { get; set; }

        [Display(Name = "生日")]
        [Required(ErrorMessage = "{0}必填")]
        [DateTimeRange(-100, -18, ErrorMessage = "生日不可早於100年,晚於18年!")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "性別")]
        [Required(ErrorMessage = "{0}必填")]
        public string? Gender { get; set; }

        [Display(Name = "帳號")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(15, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Account { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(65, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Password { get; set; }

        [Display(Name = "手機號碼")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(20, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }

        [Display(Name = "地址")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Address { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string? Email { get; set; }

        [Display(Name = "頭像")]
        public string? Avatar { get; set; }
    }

    public class DateTimeRangeAttribute : ValidationAttribute
    {
        private readonly DateTime _minDate;
        private readonly DateTime _maxDate;

        public DateTimeRangeAttribute(int initialyear, int finalyear)
        {
            _minDate = DateTime.Now.AddYears(initialyear);
            _maxDate = DateTime.Now.AddYears(finalyear);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue < _minDate)
                {
                    return new ValidationResult($"生日不可早於 {_minDate:yyyy-MM-dd}",
                        new[] { validationContext.MemberName });
                }
                else if (dateValue > _maxDate)
                {
                    return new ValidationResult($"生日不可晚於 {_maxDate:yyyy-MM-dd}",
                        new[] { validationContext.MemberName });
                }
            }
            return ValidationResult.Success;
        }
    }

    public static class MemberVMTransfer
    {
        public static MemberVM ToVM(this MemberDto dto)
        {
            return new MemberVM
            {
                MemberId = dto.MemberId,
                MemberName = dto.MemberName,
                NickName = dto.NickName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender ? "male" : "female",
                Account = dto.Account,
                Phone = dto.Phone,
                City = dto.City,
                District = dto.District,
                Address = dto.Address,
                Email = dto.Email,
                Salt = dto.Salt,
                IsActivated = dto.IsActivated,
                ConfirmGuid = dto.ConfirmGuid,
            };
        }

        public static MemberDto ToDto(this MemberCreateVM vm)
        {
            return new MemberDto
            {
                MemberId = vm.MemberId,
                MemberName = vm.MemberName,
                NickName = vm.NickName,
                DateOfBirth = vm.DateOfBirth,
                Gender = vm.Gender == "male",
                Account = vm.Account,
                Password = vm.Password,
                Phone = vm.Phone,
                City = vm.City,
                District = vm.District,
                Address = vm.Address,
                Email = vm.Email,

            };
        }

        public static MemberEditVM ToEditVM(this MemberDto dto)
        {
            return new MemberEditVM
            {
                MemberId = dto.MemberId,
                Account = dto.Account,
                NickName = dto.NickName,
                Password = dto.Password,
                Phone = dto.Phone,
                City = dto.City,
                District = dto.District,
                Address = dto.Address,
                Email = dto.Email,
                Avatar = dto.Avatar,
            };
        }

        public static MemberCenterEditVM ToCenterEditVM(this MemberEditDto dto)
        {
            return new MemberCenterEditVM
            {
                NickName = dto.NickName,
                Password = dto.Password,
                Phone = dto.Phone,
                City = dto.City,
                District = dto.District,
                Address = dto.Address,
                Email = dto.Email,
                Avatar = dto.Avatar,
            };
        }

        public static MemberEditDto ToCenterEditDto(this MemberCenterEditVM vm)
        {
            return new MemberEditDto
            {
                NickName = vm.NickName,
                Phone = vm.Phone,
                City = vm.City,
                District = vm.District,
                Address = vm.Address,
                Email = vm.Email,
                Avatar = vm.Avatar,
                IsActivated = true,
            };
        }
        
    }
}
