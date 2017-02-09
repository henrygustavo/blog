namespace Blog.WebApiSite.Core
{
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    public class EmailService : IIdentityMessageService
    {
        //public Task SendAsync(IdentityMessage message)
        //{
        //    // Plug in your email service here to send an email.
        //    return Task.FromResult(0);
        //}
        public async Task SendAsync(IdentityMessage message)
        {
            //await configSendGridasync(message);
            await GmailSendAsync(message);
        }

        //// Use NuGet to install SendGrid (Basic C# client lib) 
        //private async Task configSendGridasync(IdentityMessage message)
        //{
        //    var myMessage = new SendGridMessage();

        //    myMessage.AddTo(message.Destination);
        //    myMessage.From = new System.Net.Mail.MailAddress("henrygustavof@gmail.com", "Henry Fuentes");
        //    myMessage.Subject = message.Subject;
        //    myMessage.Text = message.Body;
        //    myMessage.Html = message.Body;

        //    var credentials = new NetworkCredential(,
        //                                            );

        //    // Create a Web transport for sending email.
        //    var transportWeb = new Web(credentials);

        //    // Send the email.
        //    if (transportWeb != null)
        //    {
        //        await transportWeb.DeliverAsync(myMessage);
        //    }
        //    else
        //    {
        //        //Trace.TraceError("Failed to create Web transport.");
        //        await Task.FromResult(0);
        //    }
        //}

        private static async Task GmailSendAsync(IdentityMessage message)
        {
            //var sentFrom = (string.IsNullOrEmpty(from) ? "my.service.email.address@gmail.com" : "my.service");

            // Configure the client:
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Creatte the credentials:
            NetworkCredential credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"], ConfigurationManager.AppSettings["emailService:Password"]);
            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail = new MailMessage(ConfigurationManager.AppSettings["emailService:Account"], message.Destination);
            mail.Subject = message.Subject;
            mail.IsBodyHtml = true;
            mail.Body = message.Body;

            await client.SendMailAsync(mail);
        }
    }
}
