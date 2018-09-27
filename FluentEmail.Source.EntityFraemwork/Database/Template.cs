using System;

namespace FluentEmail.Source.EntityFraemwork.Database
{
    public class Template
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string HtmlBodyTemplate { get; set; }

        public string PlainBodyTemplate { get; set; }

        public EmailMessagePriority Priority { get; set; }

        public string Tag { get; set; }

        public string Language { get; set; }

        public string FromAddress { get; set; }

        public bool IsHtml { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
