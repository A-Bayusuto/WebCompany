using MailKit.Security;
namespace KnifeCompany.Models
{
    public class AppSettings
    {

        public AppSettings()
        {
            Host_SecureSocketOptions = SecureSocketOptions.Auto;
        }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUser { get; set; }

        public string SmtpPass { get; set; }

        public SecureSocketOptions Host_SecureSocketOptions { get; set; }

        public string Sender_Email { get; set; }

        public string Sender_Name { get; set; }
    }
}