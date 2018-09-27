using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using FluentEmail.Source.Core;
using FluentEmail.Source.Core.Interfaces;
using FluentEmail.Source.EntityFraemwork.Database;
using FluentEmail.Source.EntityFraemwork.Managers;
using FluentEmail.Source.EntityFraemwork.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FluentEmail.Source.Simple
{
    class Program
    {
        private static readonly ITemplatesManager TemplatesManager;

        static Program()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TemplatesDbContext>();
            optionsBuilder.UseSqlServer("Server=POTOTSKY\\SQLEXPRESS;Database=Templates;Trusted_Connection=True;MultipleActiveResultSets=True;Integrated Security=true");
            var context = new TemplatesDbContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            var templatesRepository = new TemplatesRepository(context);
            TemplatesManager = new TemplatesManager(templatesRepository);
        }

        static void Main(string[] args)
        {

            var template = new DefaultTemplate<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "test template 1",
                Subject = "subject of tests template",
                HtmlBodyTemplate = "body of tests temp-late",
                PlainBodyTemplate = "hi)",
                Priority = Priority.Normal,
                Tag = "test tag",
                IsHtml = false,
                Language = CultureInfo.CurrentCulture,
                FromAddress = "test@gmail.com",
            };

            CreateTemplate(template).Wait();
            GetAndSentTemplate(template.Id).Wait();
        }

        private static async Task CreateTemplate(ITemplate<Guid> template)
        {
            await TemplatesManager.Create(template);
        }

        private static async Task Send(ITemplate<Guid> template)
        {
            var email = new Email(new RazorRenderer(), new SmtpSender(new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("test@gmail.com", "1234"),
                EnableSsl = true
            }));

            await email.UseTemplate(() => template).To("kirill.pototsky@cactussoft.biz").SendAsync();
        }

        private static async Task GetAndSentTemplate(Guid templateId)
        {
            var template = await TemplatesManager.GetById(templateId);
            await Send(template);
        }
    }
}
