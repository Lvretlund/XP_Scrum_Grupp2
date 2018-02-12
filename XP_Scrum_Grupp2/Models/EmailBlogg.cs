using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Quartz;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web.UI;
using System.Configuration;
using System.Threading.Tasks;
using XP_Scrum_Grupp2.Models;

namespace XP_Scrum_Grupp2
{
    public class EmailBlogg : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var post = db.FormalBlogs.ToList();

                mailMessage.From =
                new MailAddress(ConfigurationManager.AppSettings["FromMail"]);

                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["FromMail"]);
                mailMessage.Subject = "Daily blog notification" + DateTime.Now;
                mailMessage.Body = post.Count() + " new blog posts were added today in the formal blog";

                var allUsers = db.Users.ToList();
                foreach (var user in allUsers)
                {
                    if (user.NewFormalPostsNotification)
                    {
                        mailMessage.To.Add(new MailAddress(user.Email));
                    }
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ConfigurationManager.AppSettings["FromMail"];
                NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                smtp.EnableSsl = true;
                smtp.Send(mailMessage);
            }
        }
    }
}