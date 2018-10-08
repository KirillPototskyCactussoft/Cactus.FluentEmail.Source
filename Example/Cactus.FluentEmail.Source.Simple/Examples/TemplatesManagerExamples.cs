using System.Globalization;
using System.Threading.Tasks;
using Cactus.FluentEmail.Source.Core;
using Cactus.FluentEmail.Source.EntityFraemwork.Database;
using Cactus.FluentEmail.Source.EntityFraemwork.Managers;
using Cactus.FluentEmail.Source.EntityFraemwork.Repositories;
using FluentEmail.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cactus.FluentEmail.Source.Simple.Examples
{
    public static class TemplatesManagerExamples
    {
        private static readonly ITemplatesManager TemplatesManager;

        private static readonly string TemplateName = "Test template";
        private static readonly CultureInfo TemplateLanguage = new CultureInfo("en");

        static TemplatesManagerExamples()
        {
            var context = ConfigigureDbContext();
            TemplatesManager = new TemplatesManager(new TemplatesRepository(context));
        }

        public static async Task GetByName()
        {
            var template = await TemplatesManager.GetByName(TemplateName);

            //you can filter by language
            template = await TemplatesManager.GetByName(TemplateName, TemplateLanguage);
        }

        public static async Task Create()
        {
            var template = new DefaultTemplate
            {
                Name = TemplateName,
                Subject = "Welcom dear friend",
                HtmlBodyTemplate = "Welcome to the site",
                FromAddress = "site@gmail.com",
                Priority = Priority.Normal,
                Language = TemplateLanguage
            };

            await TemplatesManager.Create(template);
        }

        public static async Task Update()
        {
            //at first we need to get template
            var template = await TemplatesManager.GetByName(TemplateName);

            //after get we can change
            template.HtmlBodyTemplate = "updated template message";

            //to apply changes
            await TemplatesManager.Update(template.Name, template);
        }

        public static async Task Remove()
        {
            await TemplatesManager.Remove(TemplateName);
        }

        private static TemplatesDbContext ConfigigureDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TemplatesDbContext>();
            optionsBuilder.UseInMemoryDatabase("Templates");
            var context = new TemplatesDbContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
