using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class MemberFrontEditVM
    {
        [Display(Name = "編號")]
        public int MemberID { get; set; }

        [Display(Name = "暱稱")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string NickName { get; set; }

        [Display(Name = "密碼")]        
        [MaxLength(65, ErrorMessage = "{0}長度不可多於{1}")]
        public string Password { get; set; }

        [Display(Name = "手機號碼")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(20, ErrorMessage = "{0}長度不可多於{1}")]
        public string Phone { get; set; }

        [Display(Name = "地址")]
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string Address { get; set; }

        [Display(Name = "Email")]        
        [MaxLength(30, ErrorMessage = "{0}長度不可多於{1}")]
        public string Email { get; set; }

        [Display(Name = "頭像")]
        public string Avatar { get; set; }
    }
}
