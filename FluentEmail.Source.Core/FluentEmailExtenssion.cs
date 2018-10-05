using System;
using FluentEmail.Core;
using FluentEmail.Source.Core.Interfaces;
using FluentEmail.Source.Core.Logging;

namespace FluentEmail.Source.Core
{
    public static class FluentEmailExtenssion
    {
        private static readonly ILog Logger = LogProvider.GetLogger(typeof(FluentEmailExtenssion));

        public static IFluentEmail UseTemplate(this IFluentEmail fluentEmail, Func<ITemplate> getTemplate, object htmlModel = null, object plainModel = null)
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

                if (htmlModel != null && string.IsNullOrEmpty(template.HtmlBodyTemplate))
                {
                    errorMessage = "Html model was set but html template wasn't set";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(template.HtmlBodyTemplate), errorMessage);
                }

                if (plainModel != null && string.IsNullOrEmpty(template.PlainBodyTemplate))
                {
                    errorMessage = "Plain model was set but plain template wasn't set";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(template.PlainBodyTemplate), errorMessage);
                }

                fluentEmail = FillFluentEmail(template, fluentEmail, htmlModel, plainModel);

                return fluentEmail;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to apply template");
                throw;
            }
        }

        private static IFluentEmail FillFluentEmail(ITemplate template, IFluentEmail fluentEmail, object htmlModel = null, object plainModel = null)
        {
            if (!string.IsNullOrEmpty(template.HtmlBodyTemplate))
            {
                if (htmlModel != null)
                {
                    fluentEmail.UsingTemplate(template.HtmlBodyTemplate, htmlModel);
                }
                else
                {
                    fluentEmail.Body(template.HtmlBodyTemplate, true);
                }
            }

            if (!string.IsNullOrEmpty(template.PlainBodyTemplate))
            {
                if (plainModel != null)
                {
                    fluentEmail.PlaintextAlternativeUsingTemplate(template.PlainBodyTemplate, plainModel);
                }
                else
                {
                    fluentEmail.PlaintextAlternativeBody(template.PlainBodyTemplate);
                }
            }

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
