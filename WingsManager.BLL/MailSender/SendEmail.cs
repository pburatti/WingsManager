using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using WingsManager.Model.Configurations;

namespace WingsManager.BLL.MailSender
{
    public interface ISendEmail
    {
        Task<string> SendMessage(string subject, string to, string from, string body, string allegati);
    }

    public class SendEmail : ISendEmail
    {
        private readonly IOptions<AppConfiguration> _appConfig;

        public SendEmail(IOptions<AppConfiguration> appConfig)
        {
            _appConfig = appConfig;
        }
        public async Task<string> SendMessage(string subject,string to,string from, string body, string allegati)
        {
            string ret = string.Empty;

            using (SmtpClient smtpClient = new SmtpClient(_appConfig.Value.EmailConfig.ServerSmtp, int.Parse(_appConfig.Value.EmailConfig.Port)))
            {
                to = string.IsNullOrEmpty(to) ? _appConfig.Value.EmailConfig.To : to;
                from = string.IsNullOrEmpty(from) ? _appConfig.Value.EmailConfig.From : from;

                using (var message = new MailMessage() { Subject = subject, Body = body })
                {
                    foreach (var address in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        message.To.Add(address);
                    }

                    foreach (var attach in allegati.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (File.Exists(attach))
                            message.Attachments.Add(new Attachment(attach));
                    }

                    message.From = new MailAddress(from);
                    try
                    {
                        await smtpClient.SendMailAsync(message);
                        ret = "Email correctly sent";
                    }
                    catch (Exception ex)
                    {
                        ret = $"Error in sending the email to: {to}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}";
                    }
                }
            }
            
            return ret;
        }
    }

    //public class Utility
    //{
    //    public static void CheckFileOpen(FileInfo fi)
    //    {
    //        if (!File.Exists(fi.FullName)) return;
    //        bool result = false;

    //        while (!result)
    //        {
    //            FileStream stream = null;

    //            try
    //            {
    //                stream = fi.Open(FileMode.Open, FileAccess.Read, FileShare.None);

    //                result = true;
    //            }
    //            catch (IOException)
    //            {
    //                result = false;
    //            }
    //            finally
    //            {
    //                if (stream != null)
    //                    stream.Close();
    //            }
    //        }
    //    }

    //    public static bool FileIsBusy(FileInfo fi)
    //    {
    //        bool isUsed = false;
    //        if (fi.Length == 0)
    //            return true;
    //        FileStream stream = null;

    //        try
    //        {
    //            stream = fi.Open(FileMode.Open, FileAccess.Read, FileShare.None);
    //        }
    //        catch (IOException)
    //        {
    //            isUsed = true;
    //        }
    //        finally
    //        {
    //            if (stream != null)
    //                stream.Close();
    //        }

    //        return isUsed;
    //    }
    //}
}
