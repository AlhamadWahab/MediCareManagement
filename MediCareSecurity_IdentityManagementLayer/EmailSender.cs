
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MediCareSecurity_IdentityManagementLayer
{
    public class EmailSender : IEmailSender
    {
        public /*async*/ Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //var userMail = "dev70tec@gmx.de";
            //var userPass = "your-email-password";

            //var theMsg = new MailMessage();
            //theMsg.From = new MailAddress(userMail);
            //theMsg.Subject = subject;
            //theMsg.To.Add(email);
            //theMsg.Body = $"<html><body>{htmlMessage}</body></html>";
            //theMsg.IsBodyHtml = true;

            //var smtpClient = new SmtpClient("smtp-mail.outlook.com")// this string through searching on Internet: zb: hotline smtp setting.
            //{
            //    EnableSsl = true,
            //    Credentials = new NetworkCredential(userMail, userPass),
            //    Port = 587 // also when you search 
            //};
            //smtpClient.Send(theMsg);
            return Task.CompletedTask;
        }
    }
}
