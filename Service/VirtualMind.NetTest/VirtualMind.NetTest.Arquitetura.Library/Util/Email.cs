using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace VirtualMind.NetTest.Arquitetura.Library.Util
{
    public class Email
    {
        public static bool sendEmail(string body, string subject, string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("atendimento@polifrete.com", "Polifrete");
                mail.To.Add(email);
                mail.Subject = "Polifrete - " + subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("atendimento@polifrete.com", "#$%as890od!091");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
