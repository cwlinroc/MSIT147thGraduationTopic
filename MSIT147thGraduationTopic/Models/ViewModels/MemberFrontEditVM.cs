using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class MemberFrontEditVM
    {
        private Member _member = null;
        public Member Member
        {
            get { return _member; }
            set { _member = value; }
        }

        public MemberFrontEditVM()
        {
            _member = new Member();
        }

        [Display(Name = "編號")]
        public int MemberId
        {
            get { return _member.MemberId; }
            set { _member.MemberId = value; }
        }

        [Display(Name = "暱稱")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string NickName
        {
            get { return _member.NickName; }
            set { _member.NickName = value; }
        }

        [Display(Name = "密碼")]
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
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string Email
        {
            get { return _member.Email; }
            set { _member.Email = value; }
        }

        [Display(Name = "頭像")]
        public string Avatar
        {
            get { return _member.Avatar; }
            set { _member.Avatar = value; }
        }
    }

    public static partial class Members
    {
        public static MemberFrontEditVM ToEditVM(this MemberDto dto)
        {
            return new MemberFrontEditVM
            {
                MemberId = dto.MemberId,
                NickName = dto.NickName,
                Password = dto.Password,
                Phone = dto.Phone,
                Address = dto.Address,
                Email = dto.Email,
                Avatar = dto.Avatar,
            };
        }
    }
}
