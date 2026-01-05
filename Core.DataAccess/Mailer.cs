using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using com.fabioscagliola.Core.Data;

namespace com.fabioscagliola.Core.DataAccess
{
    public class Mailer : IDisposable
    {
        protected SmtpClient smtpClient;

        public static bool IsMailAddressValid(string address)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(address);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Mailer(string host, int port)
        {
            smtpClient = new SmtpClient(host, port);
        }

        public Mailer(string host, int port, string username, string password, bool enableSsl)
        {
            smtpClient = new SmtpClient(host, port);

            if (!string.IsNullOrEmpty(username))
            {
                smtpClient.Credentials = new NetworkCredential(username, password);
            }

            smtpClient.EnableSsl = enableSsl;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public static Mailer CreateDefaultInstance()
        {
            string host = Settings.Instance.Mailer.SmtpHost;
            int port = Settings.Instance.Mailer.SmtpPort;
            string username = Settings.Instance.Mailer.SmtpUsername;
            string password = Settings.Instance.Mailer.SmtpPassword;
            bool enableSsl = Settings.Instance.Mailer.SmtpEnableSsl;
            return new Mailer(host, port, username, password, enableSsl);
        }

        public void Dispose()
        {
            if (smtpClient != null)
            {
                smtpClient.Dispose(); // ... 3.5 ... 'System.Net.Mail.SmtpClient' does not contain a definition for 'Dispose' ... 
            }
        }

        public void SendMail(MailAddress from, MailAddressCollection to, MailAddressCollection cc, string subject, string body, params Attachment[] attachments)
        {
            MailMessage message = new MailMessage()
            {
                Body = body,
                From = from,
                Subject = subject,
            };

            foreach (MailAddress address in to)
            {
                message.To.Add(address);
            }

            if (cc != null)
            {
                foreach (MailAddress address in cc)
                {
                    message.CC.Add(address);
                }
            }

            if (attachments != null)
            {
                foreach (Attachment attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }
            }

            smtpClient.Send(message);
        }

        public void SendMail(string from, string to, string cc, string subject, string body, bool isBodyHtml, params Attachment[] attachments)
        {
            MailMessage message = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = isBodyHtml,
            };

            if (!string.IsNullOrEmpty(cc))
            {
                message.CC.Add(cc);
            }

            if (attachments != null)
            {
                foreach (Attachment attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }
            }

            smtpClient.Send(message);
        }

        public void SendMail(string from, string to, string cc, string subject, string body, params Attachment[] attachments)
        {
            SendMail(from, to, cc, subject, body, false, attachments);
        }

        public void SendMail(string from, string to, string cc, string subject, string body, params string[] attachments)
        {
            List<Attachment> attachmentList = new List<Attachment>();

            if (attachments != null)
            {
                foreach (string fileName in attachments)
                {
                    attachmentList.Add(new Attachment(fileName));
                }
            }

            SendMail(from, to, cc, subject, body, attachmentList.ToArray());
        }

    }
}

