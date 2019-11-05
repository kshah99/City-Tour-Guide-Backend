using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Net;
using System.Net.Mail;

public class clsMail
{
    public static bool SendMail(string MessageTo, string MessageBody, string Subject)
    {
        bool isSent = false;
        SmtpClient Client = new SmtpClient();
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(clsConfiguration.SMTPEmail, clsConfiguration.SMTPName);
        mailMessage.To.Add(MessageTo);
        mailMessage.Subject = Subject;
        mailMessage.Body = MessageBody;
        mailMessage.IsBodyHtml = true;
        Client.Host = clsConfiguration.SMTPHost;
        Client.Port = clsConfiguration.SMTPPort;
        System.Net.NetworkCredential nc = new System.Net.NetworkCredential(clsConfiguration.EmailUserName, clsConfiguration.EmailPassword);
        Client.UseDefaultCredentials = false;
        Client.Credentials = nc;
        Client.EnableSsl = false;

        try
        {
            Client.Send(mailMessage);
            isSent = true;
        }
        catch (Exception ex)
        {
            string error = ex.Message.ToString();
            isSent = false;
        }

        return isSent;
    }
}