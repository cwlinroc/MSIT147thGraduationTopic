using static MSIT147thGraduationTopic.Models.Infra.Utility.MailSetting;

namespace MSIT147thGraduationTopic.Models.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
