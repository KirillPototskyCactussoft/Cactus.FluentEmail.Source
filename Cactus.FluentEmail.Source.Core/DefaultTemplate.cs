using System.Globalization;
using Cactus.FluentEmail.Source.Core.Interfaces;
using FluentEmail.Core.Models;

namespace Cactus.FluentEmail.Source.Core
{
    public class DefaultTemplate : ITemplate
    {
        public string Name { get; set; }

        public string Subject { get; set; }

        public string HtmlBodyTemplate { get; set; }

        public string PlainBodyTemplate { get; set; }

        public Priority Priority { get; set; }

        public string Tag { get; set; }

        public CultureInfo Language { get; set; }

        public string FromAddress { get; set; }
    }
}
