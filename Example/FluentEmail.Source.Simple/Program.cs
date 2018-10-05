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
            optionsBuilder.UseInMemoryDatabase("Templates");
            var context = new TemplatesDbContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            var templatesRepository = new TemplatesRepository(context);
            TemplatesManager = new TemplatesManager(templatesRepository);
        }

        static async Task Main(string[] args)
        {
            ITemplate template = new DefaultTemplate
            {
                Name = "test template 1",
                Subject = "subject of tests template",
                HtmlBodyTemplate = "body of tests template",
                PlainBodyTemplate = "hi)",
                Priority = Priority.Normal,
                Tag = "test tag updated",
                Language = CultureInfo.CurrentCulture,
                FromAddress = "test@gmail.com"
            };

            await CreateTemplate(template);
            template = await GetTemplate(template.Name);
            await Send(template);

            template.HtmlBodyTemplate = "updated on new text)";
            template.Subject = "subject was updated too";

            await Update(template.Name, template);
            template = await GetTemplate(template.Name);
            await Send(template);

            await Remove(template.Name);
        }

        private static async Task CreateTemplate(ITemplate template)
        {
            await TemplatesManager.Create(template);
        }

        private static async Task Send(ITemplate template)
        {
            var email = new Email(new RazorRenderer(), new SmtpSender(new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("kirill.potocki@gmail.com", "PSS30004"),
                EnableSsl = true
            }));

            await email.UseTemplate(() => template).To("kirill.pototsky@cactussoft.biz").SendAsync();
        }

        private static async Task<ITemplate> GetTemplate(string templateName)
        {
            return await TemplatesManager.GetByName(templateName);
        }

        private static async Task Update(string templateName, ITemplate templateUpdates)
        {
            await TemplatesManager.Update(templateName, templateUpdates);
        }

        private static async Task Remove(string templateName)
        {
            await TemplatesManager.Remove(templateName);
        }
    }
}
