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
                var errorMessage = "Wasn't set template";
                if (getTemplate == null)
                {
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(getTemplate), errorMessage);
                }
                var template = getTemplate();
                if (template == null)
                {
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(template), errorMessage);
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

        public static IFluentEmail UseTemplate<TBodyModel, TPlainModel>(this IFluentEmail fluentEmail, Func<ITemplate> getTemplate, TBodyModel htmlModel, TPlainModel plainModel)
        {
            try
            {
                string errorMessage;
                if (getTemplate == null)
                {
                    errorMessage = "Wasn't set template";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(getTemplate), errorMessage);
                }
                var template = getTemplate();
                if (template == null)
                {
                    errorMessage = "Wasn't set template";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(getTemplate), errorMessage);
                }

                if (htmlModel == null)
                {
                    errorMessage = "Wasn't set html model";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(htmlModel), errorMessage);
                }

                if (plainModel == null)
                {
                    errorMessage = "Wasn't set plain model";
                    Logger.Error(errorMessage);
                    throw new ArgumentNullException(nameof(plainModel), errorMessage);
                }

                if (!string.IsNullOrEmpty(template.HtmlBodyTemplate)) fluentEmail.UsingTemplate(template.HtmlBodyTemplate, htmlModel);
                if (!string.IsNullOrEmpty(template.PlainBodyTemplate)) fluentEmail.PlaintextAlternativeUsingTemplate(template.PlainBodyTemplate, plainModel);
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
