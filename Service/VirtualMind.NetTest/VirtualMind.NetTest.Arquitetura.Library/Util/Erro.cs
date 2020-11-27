using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace VirtualMind.NetTest.Arquitetura.Library.Util
{
    public class Erro
    {
        public static void RegitrarLogErro(Exception ex)
        {
            Mail("ronaldokz_33@hotmail.com", "AtendUP", "Erro Sistema", GetMessage(ex));
        }

        private static string GetMessage(Exception ex)
        {
            return "<html> " +
                    "<head> " +
                    "    <meta charset='UTF-8' /> " +
                    "    <title></title> " +
                    "    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.6/css/materialize.min.css'> " +
                    "</head> " +
                    "<body> " +
                    "    <div class='container'> " +
                    "        <div class='row'> " +
                    "            <div class='col s12 m9 l10'> " +
                    "                <div id='flow' class='section scrollspy'> " +
                    "                    <h2 class='header'>Erro Aplicação</h2> " +
                    "                    <div id='flow-text-demo' class='card-panel'> " +
                    "                        <p class=''>Ola Ronaldo! Aconteceu um erro.</p> " +
                    "                        <p class=''>Data: " + DateTime.Now + "</p> " +
                    "                        <p class=''>mensagem: " + ex.Message.ToString() + "</p> " +
                    "                    </div> " +
                    "                    <p>Visite <a href='http://www.dev37.com.br' target='_blank'>37DESENVOLVIMENTOS </a></p> " +
                    "                </div> " +
                    "            </div> " +
                    "        </div> " +
                    "    </div> " +
                    "</body> " +
                    "</html> ";
        }

        public static bool Mail(string para, string nome, string sub, string body)
        {
            var m = new MailMessage()
            {
                Subject = sub,
                Body = body,
                IsBodyHtml = true
            };

            MailAddress to = new MailAddress(para, "Recuperação de senha");
            m.To.Add(to);
            m.From = new MailAddress("atendimento@dev37.com.br", "Atendimento");
            m.Sender = to;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("ronaldollemoskz@gmail.com", "neverlan236523"),
                EnableSsl = true
            };
            try
            {
                smtp.Send(m);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
