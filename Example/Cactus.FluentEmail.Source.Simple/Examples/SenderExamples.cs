using System.Threading.Tasks;
using Cactus.FluentEmail.Source.Core;
using FluentEmail.Core;

namespace Cactus.FluentEmail.Source.Simple.Examples
{
    public static class SenderExamples
    {
        public static string Sender;
        public static string Recipient;

        public static async Task UseBaseSend()
        {
            var template = new DefaultTemplate
            {
                Subject = "Base send",
                PlainBodyTemplate = "Just text",
                HtmlBodyTemplate = "New message",
                FromAddress = Sender
            };

            var email = new Email();
            await email.UseTemplate(() => template).To(Recipient).SendAsync();
        }

        public static async Task SendHtmlModel()
        {
            var template = new DefaultTemplate
            {
                Subject = "Send html model",
                PlainBodyTemplate = "Just text",
                HtmlBodyTemplate = @"@model FluentEmail.Source.Simple.TemplateModel
                                        @using System
                                        @using System.Text;

                                        <html>
                                        <head>
    
                                        </head>
                                        <body>
                                            Hi @Model.Name
                                        </body>
                                        </html>",
                FromAddress = Sender
            };
            var htmlModel = new TemplateModel { Name = "User" };

            var email = new Email();
            await email.UseTemplate(() => template, htmlModel).To(Recipient).SendAsync();
        }

        public static async Task SendPlainModel()
        {
            var template = new DefaultTemplate
            {
                Subject = "Send plain model",
                PlainBodyTemplate = @"@model FluentEmail.Source.Simple.TemplateModel
                                        @using System
                                        @using System.Text;

                                        Hi @Model.Name",
                HtmlBodyTemplate = "New Message",
                FromAddress = Sender
            };
            var plainlModel = new TemplateModel { Name = "User" };

            var email = new Email();
            await email.UseTemplate(() => template, null, plainlModel).To(Recipient).SendAsync();
        }

        public static async Task SendHtmlAndPlainModels()
        {
            var template = new DefaultTemplate
            {
                Subject = "Send html and plain model",
                PlainBodyTemplate = @"@model FluentEmail.Source.Simple.TemplateModel
                                        @using System
                                        @using System.Text;

                                        Hi @Model.Name",
                HtmlBodyTemplate = @"@model FluentEmail.Source.Simple.TemplateModel
                                        @using System
                                        @using System.Text;

                                        <html>
                                        <head>
    
                                        </head>
                                        <body>
                                            Hi @Model.Name
                                        </body>
                                        </html>",
                FromAddress = Sender
            };

            var model = new TemplateModel { Name = "User" };
            var email = new Email();
            await email.UseTemplate(() => template, model, model).To(Recipient).SendAsync();
        }
    }
}
