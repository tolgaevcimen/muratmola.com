using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace muratmola.com.Controllers
{
    public class HomeController : Controller
    {
        public string WebsiteName = ConfigurationManager.AppSettings["websiteName"];
        public string SenderMail = ConfigurationManager.AppSettings["senderMail"];
        public string SenderPassword = ConfigurationManager.AppSettings["senderPassword"];
        public string Receivers = ConfigurationManager.AppSettings["receiverMails"];

        public ActionResult Index()
        {
            return View();
        }
        
        public void Contact(string name, string email, string msg)
        {
            var mail = new MailMessage();

            foreach (var receiver in Receivers.Split(','))
            {
                mail.To.Add(new MailAddress(receiver));
            }
            mail.From = new MailAddress(email);
            mail.ReplyToList.Add(email);
            mail.Subject = $"{WebsiteName} - {name}'den bir soru var.";
            mail.Body = $"İsim:\t{name}\nEmail:\t{email}\n\n\"{msg}\"";

            SendMail(mail);
        }

        void SendMail(MailMessage Message)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.googlemail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(SenderMail, SenderPassword);
            client.Send(Message);
        }
    }
}