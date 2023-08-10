using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MSIT147thGraduationTopic.Models.Interfaces;
using static MSIT147thGraduationTopic.Models.Infra.Utility.MailSetting;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class SendMailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public SendMailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        async Task IMailService.SendEmailAsync(MailRequest mailRequest)
        {
            // 寄/收件人資訊
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject; // 主題
            //=============================================================
            //發送內容
            var builder = new BodyBuilder();
            
            builder.HtmlBody = mailRequest.Body; // 郵件訊息內容
            email.Body = builder.ToMessageBody();
            //=============================================================
            //smtp的寄送方式(使用appsetting.json的資訊)
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
