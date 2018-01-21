using System.Net;
using System.Net.Mail;
using GIR.Integracao.Fakes.Interace;
using GIR.Integracao.Model;

namespace GIR.Integracao.Fakes
{
    public class ServicoEmailGmail : IServicoEmail
    {
        public override void EnviarEmail(EstruturaEmail mensagem)
        {
            mensagem.Remetente = "timerobusto@gmail.com";
            const string senhaEmail = "stefanini@10";

            var fromAddress = new MailAddress(mensagem.Remetente, "Time Robusto Proponente");
            var adressCollection = new MailAddressCollection();
            foreach (var endereco in mensagem.Destinatarios)
            {
                adressCollection.Add(endereco);
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, senhaEmail)
            };

            using (var message = new MailMessage
            {
                Subject = mensagem.Assunto,
                Body = mensagem.Corpo,
                IsBodyHtml = true

            })
            {
                message.From = fromAddress;
                message.To.Add(adressCollection.ToString());
                smtp.Send(message);
            }
        }
    }
}
