using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace ClientsApp.Models
{
    public class Notification
    {
        public void SendEmail(string from, string to, string subject, string body)
        {

            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["AdminUser"], WebConfigurationManager.AppSettings["AdminPassword"]);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(from);

                    smtpClient.Host = WebConfigurationManager.AppSettings["SMTPName"];
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;

                    message.From = fromAddress;
                    message.Subject = subject;
                    // Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = true;
                    message.Body = body;
                    message.To.Add(to);
                    message.Bcc.Add("getquotemoving@gmail.com");

                    try
                    {
                        smtpClient.Send(message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

        }

    }
}