using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Support110Media.Utils.Helper
{
    public static class HelperMethods
    {
        /// <summary>
        /// Şifreyi MD5lemek için.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Md5Encrypt(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string val = String.Empty;
                MD5CryptoServiceProvider md5Cyripto = new MD5CryptoServiceProvider();
                byte[] bytes = Encoding.ASCII.GetBytes(value);
                byte[] arrays = md5Cyripto.ComputeHash(bytes);
                int capacity = (int)Math.Round((double)(arrays.Length * 3) + (double)arrays.Length / 8);
                StringBuilder builder = new StringBuilder(capacity);
                int num = arrays.Length - 1;
                for (int i = 0; i <= num; i++)
                {
                    builder.Append(BitConverter.ToString(arrays, i, 1));
                }
                val = builder.ToString().TrimEnd(new char[] { ' ' });
                return val;
            }
            return null;
        }

        /// <summary>
        /// Mail Gönderir
        /// </summary>
        /// <param name="mailAddress"></param>
        public static void SendMail(string mailAddress)
        {
            string url = Environment.GetEnvironmentVariable("URI") + "Support/SupportIndex";
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Connect(Environment.GetEnvironmentVariable("MAIL_IP"),
                    Convert.ToInt32(Environment.GetEnvironmentVariable("MAIL_PORT")));
                smtpClient.Authenticate(new SaslMechanismLogin(Environment.GetEnvironmentVariable("MAIL_ADDRESS"),
                    Environment.GetEnvironmentVariable("MAIL_PASSWORD")));

                //kişisel mail adres doğrulama SaslMechanismNtlm

                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("110 Media ", Environment.GetEnvironmentVariable("MAIL_ADDRESS")));
                message.To.Add(new MailboxAddress(mailAddress));
                message.Subject = "110 Media Support";

                BodyBuilder bodyBuilder = new BodyBuilder();

                string template = string.Format(
                        @"<html><p style=""text-align: center;""><strong>110Media Reklam ve Danışmanlık Ajansı</strong></p>
                            <p>Lead generation hizmeti üzerinden size yönlendirilmiş müşterilerinizi teyit etmek için <a href={0}>tıklayınız.</a></p>
                            <p>Bizi tercih ettiğiniz için teşekkür ederiz.</p></html>", url);

                bodyBuilder.HtmlBody = template;
                message.Body = bodyBuilder.ToMessageBody();
                smtpClient.Send(message);

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "MailSend");
            }
        }
    }
}
