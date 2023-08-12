using Microsoft.AspNetCore.Mvc;
using static MSIT147thGraduationTopic.Models.Infra.Utility.MailSetting;

namespace MSIT147thGraduationTopic.Models.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        string CreateUrl(string account, IUrlHelper url, string action, string controller);
    }
}
