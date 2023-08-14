using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Interfaces;
using static MSIT147thGraduationTopic.Models.Infra.Utility.MailSetting;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class SendMailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly GraduationTopicContext _context;

        public SendMailService(GraduationTopicContext context,IOptions<MailSettings> mailSettings)
        {
            _context = context;
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
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

        public string CreateUrl(string account, IUrlHelper url, string action, string controller)
        {
            //產生確認碼
            string token = Guid.NewGuid().ToString();

            var member = _context.Members.FirstOrDefault(a => a.Account == account);
            if (member != null)
            {
                member.ConfirmGuid = token;
                _context.SaveChanges();
            }

            string scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(action, controller, new { token }, scheme);
        }
    }
}
