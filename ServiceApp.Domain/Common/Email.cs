using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ServiceApp.Domain.Common
{
    public static class Email
    {
        static string strSMTPHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
        static string strSMTPPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
        static string strAdminEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString();
        static string strAdminPassword = ConfigurationManager.AppSettings["AdminEmailPassword"].ToString();

        public static void SendEmail(string strFromEmail, string strToEmail, string strSubject, string strBody)
        {
            try
            {
                using (MailMessage mm = new MailMessage(strFromEmail, strToEmail))
                {
                    mm.Subject = strSubject;
                    mm.Body = strBody;
                    //if (fuAttachment.HasFile)
                    //{
                    //    string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
                    //    mm.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
                    //}
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = strSMTPHost;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(strAdminEmail, strAdminPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = Convert.ToInt32(strSMTPPort);
                    smtp.Send(mm);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
