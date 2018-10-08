using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;

namespace Cactus.FluentEmail.Source.Simple
{
    public static class FluentEmailConfigurator
    {
        public static void Configure()
        {
            Email.DefaultSender = new SmtpSender(new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("test@gmail.com", "1234"),
                EnableSsl = true
            });
            Email.DefaultRenderer = new RazorRenderer();
        }
    }
}
