using MailKit.Net.Smtp;
using MimeKit;
using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.GestorAplicacao
{
    /// <summary>
    /// Esta classe tem como objectivo a agragação de todos os metodos necessarios para envio de Email
    /// </summary>
    public class GestorEmail
    {
        /// <summary>
        /// Atributos privados da classe GestorEmail
        /// Estes atributos são as credenciais para a autenticação do servidor no servidor de SMTP
        /// </summary>
        private string ServerEmail = "cimobgrupo2@gmail.com";
        private string EmailPassword = "gruposw2017";
        /// <summary>
        /// Construtor do GestorEmail
        /// </summary>
        public GestorEmail() {

        }
        /// <summary>
        /// Esta função tem como objectivo envio de um email para um determinado utilizador.
        /// </summary>
        /// <param name="user">Parametro do tipo Utilizador</param>
        /// <param name="titulo" type="string">Assunto do Email</param>
        /// <param name="body" type="string">Corpo do Email</param>
        public void EnviarEmail(Utilizador user,string titulo,string body) {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Server side", ServerEmail));
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
                client.Authenticate(ServerEmail, EmailPassword);
                client.Send(message);
                client.Disconnect(true);
            }

        }
    }
}
