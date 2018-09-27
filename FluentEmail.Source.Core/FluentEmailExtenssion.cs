using System;
using FluentEmail.Core;
using FluentEmail.Source.Core.Interfaces;
using FluentEmail.Source.Core.Logging;

namespace FluentEmail.Source.Core
{
    public static class FluentEmailExtenssion
    {
        private static readonly ILog Logger = LogProvider.GetLogger(typeof(FluentEmailExtenssion));

        public static IFluentEmail UseTemplate<TKey>(this IFluentEmail fluentEmail, Func<ITemplate<TKey>> getTemplate, bool isBodyHtml = false)
        {
            try
            {
                var template = getTemplate();

                if (!string.IsNullOrEmpty(template.HtmlBodyTemplate))
                {
                    fluentEmail.Body(template.HtmlBodyTemplate, isBodyHtml);
                }

                if (!string.IsNullOrEmpty(template.PlainBodyTemplate))
                {
                    fluentEmail.PlaintextAlternativeBody(template.PlainBodyTemplate);
                }

                fluentEmail = FillFluentEmail(template, fluentEmail);

                return fluentEmail;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to apply template");
                throw;
            }
        }

        public static IFluentEmail UseTemplate<TKey, TBodyModel, TPlainModel>(this IFluentEmail fluentEmail, Func<ITemplate<TKey>> getTemplate, TBodyModel bodyModel, TPlainModel plainModel, bool isBodyHtml = true)
        {
            try
            {
                var template = getTemplate();

                if (!string.IsNullOrEmpty(template.HtmlBodyTemplate))
                {
                    fluentEmail.UsingTemplate(template.HtmlBodyTemplate, bodyModel, isBodyHtml);
                }

                if (!string.IsNullOrEmpty(template.PlainBodyTemplate))
                {
                    fluentEmail.PlaintextAlternativeUsingTemplate(template.PlainBodyTemplate, plainModel);
                }

                fluentEmail = FillFluentEmail(template, fluentEmail);

                return fluentEmail;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to apply template");
                throw;
            }
        }

        public static IFluentEmail UseTemplateWithPlainModel<TKey, TPlainModel>(this IFluentEmail fluentEmail, Func<ITemplate<TKey>> getTemplate, TPlainModel plainModel, bool isBodyHtml = false)
        {
            try
            {
                var template = getTemplate();

                if (!string.IsNullOrEmpty(template.HtmlBodyTemplate))
                {
                    fluentEmail.Body(template.HtmlBodyTemplate, isBodyHtml);
                }

                if (!string.IsNullOrEmpty(template.PlainBodyTemplate))
                {
                    fluentEmail.PlaintextAlternativeUsingTemplate(template.PlainBodyTemplate, plainModel);
                }

                fluentEmail = FillFluentEmail(template, fluentEmail);

                return fluentEmail;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to apply template");
                throw;
            }
        }

        public static IFluentEmail UseTemplateWithBodyModel<TKey, TBodyModel>(this IFluentEmail fluentEmail, Func<ITemplate<TKey>> getTemplate, TBodyModel bodyModel, bool isBodyHtml = true)
        {
            try
            {
                var template = getTemplate();

                if (!string.IsNullOrEmpty(template.HtmlBodyTemplate))
                {
                    fluentEmail.UsingTemplate(template.HtmlBodyTemplate, bodyModel, isBodyHtml);
                }

                if (!string.IsNullOrEmpty(template.PlainBodyTemplate))
                {
                    fluentEmail.PlaintextAlternativeBody(template.PlainBodyTemplate);
                }

                fluentEmail = FillFluentEmail(template, fluentEmail);

                return fluentEmail;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to apply template");
                throw;
            }

        }

        private static IFluentEmail FillFluentEmail<TKey>(ITemplate<TKey> template, IFluentEmail fluentEmail)
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
