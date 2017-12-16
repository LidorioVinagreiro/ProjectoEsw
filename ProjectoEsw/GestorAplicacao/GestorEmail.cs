using MailKit.Net.Smtp;
using MimeKit;
using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.GestorAplicacao
{
    public class GestorEmail
    {

        public GestorEmail() {

        }

        public void EnviarEmail(Utilizador user,string titulo,string body) {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Server side", "cimobgrupo2@gmail.com"));
            message.To.Add(new MailboxAddress("Novo Utilizador", user.Email.ToString() ));
            message.Subject = titulo;
            //acho que tenho que user body builder
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("cimobgrupo2@gmail.com", "gruposw2017");
                client.Send(message);
                client.Disconnect(true);
            }

        }
    }
}
