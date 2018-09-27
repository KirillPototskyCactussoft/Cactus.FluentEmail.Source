﻿using System.Globalization;
using FluentEmail.Core.Models;

namespace FluentEmail.Source.Core.Interfaces
{
    public interface ITemplate<TKey>
    {
        TKey Id { get; set; }

        string Name { get; set; }

        string Subject { get; set; }

        string HtmlBodyTemplate { get; set; }

        string PlainBodyTemplate { get; set; }

        Priority Priority { get; set; }

        string Tag { get; set; }

        CultureInfo Language { get; set; }

        string FromAddress { get; set; }

        bool IsHtml { get; set; }
    }
}