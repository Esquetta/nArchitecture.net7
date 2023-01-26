using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Core.Mailing.MailKitImplementations
{
    public class MailKitMailService:IMailService
    {
        readonly IConfiguration configuration;
        readonly MailSettings mailSettings;
        public MailKitMailService(IConfiguration configuration)
        {
            this.configuration = configuration;
            mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
        }

        public void SendMail(Mail mail)
        {
            MimeMessage email = new();

           
            email.From.Add(new MailboxAddress(mailSettings.SenderFullName,mailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(mail.ToFullName, mail.ToEmail));
            email.Subject = mail.Subject;

            BodyBuilder builder = new BodyBuilder()
            {
                HtmlBody=mail.HtmlBody,
                TextBody=mail.TextBody,
            };
            if(mail.Attachment!= null)
                foreach (var item in mail.Attachment)
                    builder.Attachments.Add(item);

            email.Body=builder.ToMessageBody();

            using SmtpClient smtpClient= new SmtpClient();
            smtpClient.Connect(mailSettings.Server, mailSettings.Port);
            smtpClient.Send(email);
            smtpClient.Disconnect(true);
                
        }
    }
}
