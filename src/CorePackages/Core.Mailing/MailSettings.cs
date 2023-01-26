using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mailing
{
    public class MailSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderFullName { get; set; }
        public string SenderEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public MailSettings()
        {

        }
        public MailSettings(string Server,int Port,string SenderFullName,string SenderEmail,
            string UserName,string Password)
        {
            this.Server = Server;
            this.Port= Port;
            this.SenderFullName= SenderFullName;
            this.SenderEmail= SenderEmail;
            this.UserName= UserName;
            this.Password= Password;
        }
    }
}
