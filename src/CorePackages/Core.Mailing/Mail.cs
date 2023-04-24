using MimeKit;

namespace Core.Mailing
{
    public class Mail
    {
        public string Subject { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public AttachmentCollection Attachment { get; set; }
        public string ToFullName { get; set; }
        public string ToEmail { get; set; }

        public Mail()
        {

        }
        public Mail(string Subject,string TextBody,string HtmlBody,AttachmentCollection AttachmentCollection,
            string ToFullName,string ToEmail)
        {
            this.Subject = Subject;
            this.TextBody = TextBody;
            this.HtmlBody = HtmlBody;
            this.Subject=Subject;
            this.Attachment = AttachmentCollection;
            this.ToFullName = ToFullName;
        }
                
    }
}
