using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class MemberRegisterVM
    {
        private Member _member = null;
        public Member Member
        {
            get { return _member; }
            set { _member = value; }
        }

        public MemberRegisterVM()
        {
            _member = new Member();
        }

        [Display(Name = "編號")]
        public int MemberId
        {
            get { return _member.MemberId; }
            set { _member.MemberId = value; }
        }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string MemberName
        {
            get { return _member.MemberName; }
            set { _member.MemberName = value; }
        }

        [Display(Name = "暱稱")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string NickName
        {
            get { return _member.NickName; }
            set { _member.NickName = value; }
        }

        [Display(Name = "生日")]
        [Required(ErrorMessage = "{0}必填")]
        [DateTimeRange(-100, -18, ErrorMessage = "年齡不可大於100歲,小於18歲!")]
        public DateTime DateOfBirth
        {
            get { return _member.DateOfBirth; }
            set { _member.DateOfBirth = value; }
        }

        [Display(Name = "性別")]
        [Required(ErrorMessage = "{0}必填")]
        public bool? Gender
        {
            get { return _member.Gender; }
            set { _member.Gender = (bool)value; }
        }

        [Display(Name = "帳號")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(15, ErrorMessage = "{0}長度不可多於{1}")]
        public string Account
        {
            get { return _member.Account; }
            set { _member.Account = value; }
        }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(65, ErrorMessage = "{0}長度不可多於{1}")]
        public string Password
        {
            get { return _member.Password; }
            set { _member.Password = value; }
        }

        [Display(Name = "手機號碼")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(20, ErrorMessage = "{0}長度不可多於{1}")]
        public string Phone
        {
            get { return _member.Phone; }
            set { _member.Phone = value; }
        }

        [Display(Name = "地址")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string Address
        {
            get { return _member.Address; }
            set { _member.Address = value; }
        }

        [Display(Name = "Email")]
        [EmailAddress]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string Email
        {
            get { return _member.Email; }
            set { _member.Email = value; }
        }

    }

    public class AddressVM
    {
        public class Record
        {
            public string city { get; set; }
            public string site_id { get; set; }
            
        }
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

    public static partial class Members
    {
        public static MemberRegisterVM ToRegisterVM(this MemberDto dto)
        {
            return new MemberRegisterVM
            {
                MemberId = dto.MemberId,
                MemberName = dto.MemberName,
                NickName = dto.NickName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Account = dto.Account,
                Password = dto.Password,
                Phone = dto.Phone,
                Address = dto.Address,
                Email = dto.Email
            };
        }
    }
}
