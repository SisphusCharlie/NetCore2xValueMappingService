using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            string address = "huangxin3309suc@163.com";
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("ValueAPI", address));
            msg.To.Add(new MailboxAddress("Sir/Miss:",email));

            msg.Subject = subject;

            //msg.Body = new TextPart("plain") { Text = message };
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = @message;
            msg.Body = bodyBuilder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                //client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                //smtpClient.Connect("smtp.163.com", 25, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication

                string password = "WUXIEKEJI+1";
                smtpClient.Timeout = 10 * 1000;   //设置超时时间
                string host = "smtp.163.com";
                int port = 25;
                smtpClient.Connect(host, port, MailKit.Security.SecureSocketOptions.None);//连接到远程smtp服务器
                smtpClient.Authenticate(address, password);
                smtpClient.Send(msg);
                smtpClient.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
