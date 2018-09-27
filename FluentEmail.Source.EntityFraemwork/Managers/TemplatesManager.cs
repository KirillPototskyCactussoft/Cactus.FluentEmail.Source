using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core.Models;
using FluentEmail.Source.Core;
using FluentEmail.Source.Core.Interfaces;
using FluentEmail.Source.EntityFraemwork.Database;
using FluentEmail.Source.EntityFraemwork.Logging;
using FluentEmail.Source.EntityFraemwork.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FluentEmail.Source.EntityFraemwork.Managers
{
    public class TemplatesManager : ITemplatesManager
    {
        private readonly ITemplatesRepository _templatesRepository;
        private readonly ILog _logger = LogProvider.GetLogger(typeof(TemplatesManager));

        public TemplatesManager(ITemplatesRepository templatesRepository)
        {
            _templatesRepository = templatesRepository;
        }

        public async Task<IEnumerable<ITemplate<Guid>>> GetByLanguage(string language)
        {
            var entity = await _templatesRepository.GetQuerable().Where(x => x.Language == language).ToListAsync();
            return entity.Select(CastToDefaultTemplate);
        }

        public async Task<ITemplate<Guid>> GetById(Guid id)
        {
            var entity = await _templatesRepository.GetQuerable().FirstOrDefaultAsync(x => x.Id == id);
            return CastToDefaultTemplate(entity);
        }

        public async Task Create(ITemplate<Guid> template)
        {
            try
            {
                var entity = new Template
                {
                    Id = template.Id,
                    Name = template.Name,
                    Subject = template.Subject,
                    HtmlBodyTemplate = template.HtmlBodyTemplate,
                    PlainBodyTemplate = template.PlainBodyTemplate,
                    Priority = CastToEmailMessagePriority(template.Priority),
                    Tag = template.Tag,
                    Language = template.Language.Name,
                    FromAddress = template.FromAddress,
                    IsHtml = template.IsHtml,
                    CreatedDateTime = DateTime.UtcNow
                };

                await _templatesRepository.CreateAsync(entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Failed to create template");
                throw;
            }
        }

        public async Task Update(Guid id, ITemplate<Guid> templateUpdates)
        {
            try
            {
                var template = await _templatesRepository.GetQuerable().FirstOrDefaultAsync(x => x.Id == id);
                if (template == null)
                {
                    var erroMessage = $"Failed to update template because wasn't fond template with id {id}";
                    _logger.Error(erroMessage);
                    throw new ArgumentException(erroMessage);
                }
                template.Name = templateUpdates.Name;
                template.Subject = templateUpdates.Subject;
                template.HtmlBodyTemplate = templateUpdates.HtmlBodyTemplate;
                template.PlainBodyTemplate = templateUpdates.PlainBodyTemplate;
                template.Priority = CastToEmailMessagePriority(templateUpdates.Priority);
                template.Tag = templateUpdates.Tag;
                template.Language = templateUpdates.Language.Name;
                template.FromAddress = templateUpdates.FromAddress;
                template.IsHtml = templateUpdates.IsHtml;

                await _templatesRepository.UpdateAsync(template);
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Failed to update template");
                throw;
            }
        }

        public async Task Remove(Guid id)
        {
            try
            {
                var template = await _templatesRepository.GetQuerable().FirstOrDefaultAsync(x => x.Id == id);
                if (template == null)
                {
                    var erroMessage = $"Failed to delete template because wasn't fond template with id {id}";
                    _logger.Error(erroMessage);
                    throw new ArgumentException(erroMessage);
                }
                await _templatesRepository.RemoveAsync(template);
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Failed to delete template");
                throw;
            }
        }

        private DefaultTemplate<Guid> CastToDefaultTemplate(Template template)
        {
            return new DefaultTemplate<Guid>
            {
                Id = template.Id,
                Name = template.Name,
                Subject = template.Subject,
                HtmlBodyTemplate = template.HtmlBodyTemplate,
                PlainBodyTemplate = template.PlainBodyTemplate,
                Priority = CastToPriority(template.Priority),
                Tag = template.Tag,
                Language = new CultureInfo(template.Language),
                FromAddress = template.FromAddress,
                IsHtml = template.IsHtml
            };
        }

        private EmailMessagePriority CastToEmailMessagePriority(Priority priority)
        {
            switch (priority)
            {
                case Priority.Low: return EmailMessagePriority.Low;
                case Priority.Normal: return EmailMessagePriority.Normal;
                case Priority.High: return EmailMessagePriority.High;
            }

            var errorMessage = "Failed to define priority";
            _logger.Error(errorMessage);
            throw new ArgumentOutOfRangeException(nameof(priority), errorMessage);
        }

        private Priority CastToPriority(EmailMessagePriority emailMessagePriority)
        {
            switch (emailMessagePriority)
            {
                case EmailMessagePriority.Low: return Priority.Low;
                case EmailMessagePriority.Normal: return Priority.Normal;
                case EmailMessagePriority.High: return Priority.High;
            }

            var errorMessage = "Failed to define email message priority";
            _logger.Error(errorMessage);
            throw new ArgumentOutOfRangeException(nameof(emailMessagePriority), errorMessage);
        }
    }
}
