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
            var mail = new MailMessage
            {
                From = new MailAddress(SenderMail),
                Subject = $"{WebsiteName} - {name}'den bir soru var.",
                Body = $"İsim:\t{name}\nEmail:\t{email}\n\n\"{msg}\""
            };
            mail.ReplyToList.Add(email);
            foreach (var receiver in Receivers.Split(','))
            {
                mail.To.Add(new MailAddress(receiver));
            }

            SendMail(mail);
        }

        void SendMail(MailMessage Message)
        {
            var client = new SmtpClient
            {
                Host = "smtp.muratmola.com",
                Port = 587,
                Credentials = new NetworkCredential(SenderMail, SenderPassword)
            };
            client.Send(Message);
        }
    }
}