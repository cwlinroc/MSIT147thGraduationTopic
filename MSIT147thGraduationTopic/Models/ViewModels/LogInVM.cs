using System.ComponentModel.DataAnnotations;

namespace MSIT147thGraduationTopic.Models.ViewModels
{
    public class LogInVM
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public string Password { get; set; }
        public bool chkRemember { get; set; }
    }
}
