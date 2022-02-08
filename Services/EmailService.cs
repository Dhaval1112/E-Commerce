using E_Commerce.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Services
{


    public class EmailService : IEmailService
    {
        public EmailService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly IConfiguration _configuration;

        private async Task SendEmail(MimeMessage message, string email, string password)
        {


            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(email, password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }

        }
        public async Task sendTestEmail(UserEmailOptions options)
        {

            var email = _configuration.GetSection("SMPTConfig").GetSection("emailaddress").Value;

            var password = _configuration.GetSection("SMPTConfig").GetSection("password").Value;



            var message = new MimeMessage();

            message.Subject = UpdatedParameters(options.Subject,options.PlaceHolders);

            message.From.Add(new MailboxAddress(options.Heading, email));

            message.To.Add(new MailboxAddress("Saurin", options.ToEmail));



            BodyBuilder bodyBuilder = new BodyBuilder();

            var readFileData= GetEmailBody(options.TemplateName);
            var formatedMail = UpdatedParameters(readFileData, options.PlaceHolders);

            bodyBuilder.HtmlBody = formatedMail;

            message.Body = bodyBuilder.ToMessageBody();

            await SendEmail(message, email, password);
        }

        public string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        public string UpdatedParameters(string text,List<KeyValuePair<string,string>> keyValuePairs)
        {
            if(!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}
