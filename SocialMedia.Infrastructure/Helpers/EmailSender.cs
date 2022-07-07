﻿using MailKit.Net.Smtp;
using MimeKit;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Models;

namespace SocialMedia.Infrastructure.Helpers
{
    /// <summary>
    /// A service for email related operations.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        #region SendEmailAsync

        /// <inheritdoc cref="IEmailSender.SendEmailAsync(MailMessage)"/>
        public async Task SendEmailAsync(MailMessage mailMessage)
        {
            MimeMessage message = new MimeMessage();

            // From
            message.From.Add(new MailboxAddress("Waad Sulaiman", AppSettings.Email));
            // To
            foreach (var item in mailMessage.Recipients)
            {
                message.To.Add(new MailboxAddress(item.DisplayName, item.Address));
            }
            // Subject
            message.Subject = mailMessage.Subject;
            // Body
            message.Body = new TextPart()
            {
                Text = mailMessage.Body,
            };

            using var client = new SmtpClient();
            client.Connect(AppSettings.SmtpServer, AppSettings.SmtpPort);
            client.Authenticate(AppSettings.Email, AppSettings.Password);

            await client.SendAsync(message);
            client.Disconnect(true);
        }

        #endregion SendEmailAsync
    }
}