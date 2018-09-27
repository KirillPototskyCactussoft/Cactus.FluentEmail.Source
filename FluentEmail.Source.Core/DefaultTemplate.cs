using System.Globalization;
using FluentEmail.Core.Models;
using FluentEmail.Source.Core.Interfaces;

namespace FluentEmail.Source.Core
{
    public class DefaultTemplate<TKey> : ITemplate<TKey>
    {
        public TKey Id { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string HtmlBodyTemplate { get; set; }

        public string PlainBodyTemplate { get; set; }

        public Priority Priority { get; set; }

        public string Tag { get; set; }

        public CultureInfo Language { get; set; }

        public string FromAddress { get; set; }

        public bool IsHtml { get; set; }
    }
}
