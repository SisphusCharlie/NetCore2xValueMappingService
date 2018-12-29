using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Services.MailHelper
{
    public class EmailHelper
    {
        private ViewRenderService _viewRenderService;

        public EmailHelper(ViewRenderService vs)
        {
            _viewRenderService = vs;
        }
//         "Mail": {
//2     "Name": "博客园招聘频道",
//3     "Address": "emailAddress",
//4     "Host": "xxxxxxx",
//5     "Port": 25,
//6     "Password": "password"
//7   }
        
        //public async Task SendMail(string receive, string sender, string subject, string body, byte[] attachments = null)
        //{
        //    var userResumeAttachmentString = await _viewRenderService.RenderToStringAsync("Details", "ValueMaps");
        //    string displayName ="ValueMapping";
        //    string from = "huangxin3309suc@163.com";
        //    var fromMailAddress = new MailboxAddress(displayName, from);
        //    var toMailAddress = new MailboxAddress(receive);
        //    var mailMessage = new MimeMessage();
        //    mailMessage.From.Add(fromMailAddress);
        //    mailMessage.To.Add(toMailAddress);
        //    if (!string.IsNullOrEmpty(sender))
        //    {
        //        var replyTo = new MailboxAddress(displayName, sender);
        //        mailMessage.ReplyTo.Add(replyTo);
        //    }
        //    var bodyBuilder = new BodyBuilder() { HtmlBody = body };
        //    mailMessage.Body = bodyBuilder.ToMessageBody();
        //    mailMessage.Subject = subject;

        //    attachments = System.Text.Encoding.ASCII.GetBytes(userResumeAttachmentString);

        //    if (attachments != null)
        //    {
        //        bodyBuilder.Attachments.Add("查询结果.pdf", attachments);
        //    }


        //    await SendMail(mailMessage);

        }
        //private async Task SendMail(MimeMessage mailMessage)
        //{
        //    try
        //    {
        //        var smtpClient = new SmtpClient();
        //        smtpClient.Timeout = 10 * 1000;   //设置超时时间
        //        string host = "smtp.163.com";
        //        int port =25;
        //        string address ="huangxin3309suc@163.com";
        //        string password ="WUXIEKEJI+1";
        //        smtpClient.Connect(host, port, MailKit.Security.SecureSocketOptions.None);//连接到远程smtp服务器
        //        smtpClient.Authenticate(address, password);
        //        smtpClient.Send(mailMessage);//发送邮件
        //        smtpClient.Disconnect(true);

        //    }
        //    catch
        //    {
        //        return false;
        //    }

    //    //}
    //}
}
