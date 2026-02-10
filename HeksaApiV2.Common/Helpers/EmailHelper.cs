using HeksaApiV2.Common.Object;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace HeksaApiV2.Common.Helpers
{
    public class EmailHelper
    {
        private GlobalSettings _configs;
        private IWebHostEnvironment _env;
        public EmailHelper(IConfiguration _iconfig, IWebHostEnvironment _ienv)
        {
            _configs = new GlobalSettings(_iconfig);
            _env = _ienv;
        }

        public bool IsValidEmail(string email)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(email))
            {
                Match match = Regex.Match(email, "^([\\w-]+(?:\\.[\\w-]+)*)@((?:[\\w-]+\\.)*\\w[\\w-]{0,66})\\.([a-z]{2,6}(?:\\.[a-z]{2})?)$", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public void SendEmail(string subject, string message, List<string> Emails)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_configs.SITE_EMAIL, _configs.SMTP_NAME_DISPLAY);

            if (Emails != null)
            {
                foreach (var emailto in Emails.Distinct())
                {
                    mailMessage.To.Add(new MailAddress(emailto));
                }
            }

            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            mailMessage.Headers.Add("Message-id", string.Format("{0}{1}@heksainsurance.co.id", "Heksa", DateTime.Now.ToString("ddMMyyyyHHmmss")));
            mailMessage.Headers.Add("Date", DateTime.Now.ToString("ddMMyyyyHHmmss"));

            SmtpClient client = new SmtpClient();
            if (!string.IsNullOrEmpty(_configs.SMTP_HOST)) client.Host = _configs.SMTP_HOST;
            client.EnableSsl = bool.Parse(_configs.SMTP_SSL);
            if (!string.IsNullOrEmpty(_configs.SMTP_USERNAME)) client.Credentials = new NetworkCredential(_configs.SMTP_USERNAME, _configs.SMTP_PASSWORD);
            if (!string.IsNullOrEmpty(_configs.SMTP_PORT))
            {
                client.Port = Convert.ToInt32(_configs.SMTP_PORT);
            }

            client.Send(mailMessage);
        }

        public void SendEmailWithAttachment(string subject, string message, List<string> ListEmailTo, List<string> ListPathAttachment, Dictionary<string, Stream> DictStreamAttachment = null)
        {
            SmtpClient client = new SmtpClient();
            var mailMessage = new MailMessage();
            try
            {
                mailMessage.From = new MailAddress(_configs.SITE_EMAIL, _configs.SMTP_NAME_DISPLAY);
                if (ListEmailTo != null && ListEmailTo.Count > 0)
                {
                    foreach (var item in ListEmailTo)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            mailMessage.To.Add(new MailAddress(item));
                        }
                    }

                    if (mailMessage.To.Count == 0)
                        return;
                }
                else
                    return;

                if (ListPathAttachment != null && ListPathAttachment.Count > 0)
                {
                    foreach (var item in ListPathAttachment)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            Attachment attach = new Attachment(item);
                            mailMessage.Attachments.Add(attach);
                        }
                    }
                }

                if (DictStreamAttachment != null && DictStreamAttachment.Count > 0)
                {
                    foreach (var item in DictStreamAttachment)
                    {
                        if (item.Value != null)
                        {
                            Attachment attach = new Attachment(item.Value, item.Key, null);
                            mailMessage.Attachments.Add(attach);
                        }
                    }
                }

                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                mailMessage.Headers.Add("Message-id", string.Format("{0}{1}@heksainsurance.co.id", "Heksa", DateTime.Now.ToString("ddMMyyyyHHmmss")));
                mailMessage.Headers.Add("Date", DateTime.Now.ToString("ddMMyyyyHHmmss"));

                if (!string.IsNullOrEmpty(_configs.SMTP_HOST)) client.Host = _configs.SMTP_HOST;
                client.EnableSsl = bool.Parse(_configs.SMTP_SSL);
                if (!string.IsNullOrEmpty(_configs.SMTP_USERNAME)) client.Credentials = new NetworkCredential(_configs.SMTP_USERNAME, _configs.SMTP_PASSWORD);
                if (!string.IsNullOrEmpty(_configs.SMTP_PORT))
                {
                    client.Port = Convert.ToInt32(_configs.SMTP_PORT);
                }

                client.Send(mailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Dispose();
                mailMessage.Dispose();
            }

        }

        public string LoadTemplate(string templateName, string webAssetFolder)
        {
            string result = string.Empty;
            string pathToFile = Path.Combine(webAssetFolder + "\\email\\", templateName); //"email/partner-agen-registration.html"
            if (File.Exists(pathToFile))
            {
                using (StreamReader reader = File.OpenText(pathToFile))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
    }
}
