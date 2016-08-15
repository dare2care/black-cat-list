namespace BlackCatList.Web
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await this.ConfigSendGridAsync(message);
        }

        private async Task ConfigSendGridAsync(IdentityMessage message)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.To.Add(message.Destination);
                mailMessage.Subject = message.Subject;
                mailMessage.Body = message.Body;
                mailMessage.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Credentials = new NetworkCredential(
                        Environment.GetEnvironmentVariable("BlackCatList.Smtp.Username"),
                        Environment.GetEnvironmentVariable("BlackCatList.Smtp.Password"));

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }
}