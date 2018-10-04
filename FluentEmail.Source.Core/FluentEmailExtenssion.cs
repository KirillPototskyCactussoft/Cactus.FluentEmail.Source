using System;
using FluentEmail.Core;
using FluentEmail.Source.Core.Interfaces;
using FluentEmail.Source.Core.Logging;

namespace FluentEmail.Source.Core
{
    public static class FluentEmailExtenssion
    {
        private static readonly ILog Logger = LogProvider.GetLogger(typeof(FluentEmailExtenssion));

        public static IFluentEmail UseTemplate(this IFluentEmail fluentEmail, Func<ITemplate> getTemplate)
        {
            try
            {
                var template = getTemplate?.Invoke();
                if (template == null)
                {
                    var errorMessage = "The template wasn't set";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(getTemplate), errorMessage);
                }

                if (!string.IsNullOrEmpty(template.HtmlBodyTemplate)) fluentEmail.Body(template.HtmlBodyTemplate, true);
                if (!string.IsNullOrEmpty(template.PlainBodyTemplate)) fluentEmail.PlaintextAlternativeBody(template.PlainBodyTemplate);
                fluentEmail = FillFluentEmail(template, fluentEmail);

                return fluentEmail;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to apply template");
                throw;
            }
        }

        public static IFluentEmail UseTemplate<THtmlModel, TPlainModel>(this IFluentEmail fluentEmail, Func<ITemplate> getTemplate, THtmlModel htmlModel, TPlainModel plainModel)
        {
            try
            {
                string errorMessage;
                var template = getTemplate?.Invoke();
                if (template == null)
                {
                    errorMessage = "The template wasn't set";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(getTemplate), errorMessage);
                }

                if (htmlModel == null || string.IsNullOrEmpty(template.HtmlBodyTemplate))
                {
                    errorMessage = "Html model or html body wasn't set";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(htmlModel), errorMessage);
                }

                if (plainModel == null || string.IsNullOrEmpty(template.PlainBodyTemplate))
                {
                    errorMessage = "Plain model or plain body wasn't set";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(plainModel), errorMessage);
                }

                fluentEmail.UsingTemplate(template.HtmlBodyTemplate, htmlModel);
                fluentEmail.PlaintextAlternativeUsingTemplate(template.PlainBodyTemplate, plainModel);
                fluentEmail = FillFluentEmail(template, fluentEmail);

                return fluentEmail;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to apply template");
                throw;
            }
        }

        public static IFluentEmail UseTemplateWithHtmlModel<THtmlModel>(this IFluentEmail fluentEmail, Func<ITemplate> getTemplate, THtmlModel htmlModel)
        {
            try
            {
                var errorMessage = "The template wasn't set";
                var template = getTemplate?.Invoke();
                if (template == null)
                {
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(getTemplate), errorMessage);
                }

                if (htmlModel == null || string.IsNullOrEmpty(template.HtmlBodyTemplate))
                {
                    errorMessage = "Html model or html body wasn't set";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(htmlModel), errorMessage);
                }

                fluentEmail.UsingTemplate(template.HtmlBodyTemplate, htmlModel);
                if (!string.IsNullOrEmpty(template.PlainBodyTemplate)) fluentEmail.PlaintextAlternativeBody(template.PlainBodyTemplate);
                fluentEmail = FillFluentEmail(template, fluentEmail);

                return fluentEmail;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to apply template");
                throw;
            }
        }

        public static IFluentEmail UseTemplateWithPlainModel<TPlainModel>(this IFluentEmail fluentEmail, Func<ITemplate> getTemplate, TPlainModel plainModel)
        {
            try
            {
                var errorMessage = "The template wasn't set";
                var template = getTemplate?.Invoke();
                if (template == null)
                {
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(getTemplate), errorMessage);
                }

                if (plainModel == null || string.IsNullOrEmpty(template.PlainBodyTemplate))
                {
                    errorMessage = "Plain model or plain body wasn't set";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(plainModel), errorMessage);
                }

                if (!string.IsNullOrEmpty(template.HtmlBodyTemplate)) fluentEmail.Body(template.HtmlBodyTemplate, true);

                fluentEmail.PlaintextAlternativeUsingTemplate(template.PlainBodyTemplate, plainModel);
                fluentEmail = FillFluentEmail(template, fluentEmail);

                return fluentEmail;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to apply template");
                throw;
            }
        }

        private static IFluentEmail FillFluentEmail(ITemplate template, IFluentEmail fluentEmail)
        {
            if (!string.IsNullOrEmpty(template.Subject))
            {
                fluentEmail.Subject(template.Subject);
            }

            if (!string.IsNullOrEmpty(template.FromAddress))
            {
                fluentEmail.SetFrom(template.FromAddress);
            }

            fluentEmail.Data.Priority = template.Priority;

            if (!string.IsNullOrEmpty(template.Tag))
            {
                fluentEmail.Tag(template.Tag);
            }

            return fluentEmail;
        }
    }
}
