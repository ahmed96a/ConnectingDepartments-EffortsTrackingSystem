using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text;

namespace EffortTrackingSystem.API.Utilities
{
    public class EmailHelper
    {
        private IConfiguration _config;
        private string host;
        private string port;
        private string from;
        private string alias;

        public EmailHelper(IConfiguration Config)
        {
            _config = Config;
            host = _config["SMTP:Host"];
            port = _config["SMTP:Port"];
            from = _config["SMTP:From"];
            alias = _config["SMTP:Alias"];
        }

        public void SendEmail(EmailModel emailModel)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(host, Convert.ToInt32(port)))
                {
                    smtpClient.EnableSsl = true;

                    smtpClient.Credentials = new System.Net.NetworkCredential()
                    {
                        UserName = "applicationjobfinder@gmail.com",
                        Password = "123456@Sd"
                    };

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(from, alias);
                    mailMessage.To.Add(emailModel.To);
                    mailMessage.Body = emailModel.Body;
                    mailMessage.Subject = emailModel.Subject;
                    mailMessage.IsBodyHtml = emailModel.IsBodyHtml;
                    mailMessage.BodyEncoding = Encoding.UTF8;

                    smtpClient.Send(mailMessage);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class EmailModel
    {
        public EmailModel(string to, string subject, string body, bool isBodyHtml)
        {
            To = to;
            Subject = subject;
            Body = body;
            IsBodyHtml = isBodyHtml;
        }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }
    }
}
