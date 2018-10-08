using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Cactus.FluentEmail.Source.Core;
using Cactus.FluentEmail.Source.EntityFraemwork.Database;
using Cactus.FluentEmail.Source.EntityFraemwork.Managers;
using Cactus.FluentEmail.Source.EntityFraemwork.Repositories;
using FluentEmail.Core.Models;
using Moq;
using NUnit.Framework;

namespace Cactus.FluentEmail.Source.EntityFraemwork.Tests.Managers
{
    [TestFixture]
    public class TemplatesManagerTests
    {
        [Test]
        public async Task GetByName_Success()
        {
            //Arrange
            var templatesRepository = new Mock<ITemplatesRepository>();
            var templatesManager = new TemplatesManager(templatesRepository.Object);

            var searchingName = "tests template 1";
            var entities = new List<Template>
            {
                new Template
                {
                    Name = searchingName,
                    Subject = "tests subject 1",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBodyTemplate = "test plain",
                    Priority = EmailMessagePriority.Normal,
                    Tag = "test tag",
                    Language = "en",
                    FromAddress = "test1@gmail.com",
                    CreatedDateTime = DateTime.UtcNow
                },
                new Template
                {
                    Name = "tests template 3",
                    Subject = "tests subject 2",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBodyTemplate = "test plain",
                    Priority = EmailMessagePriority.Low,
                    Tag = "test tag from kirill)",
                    Language = "fz",
                    FromAddress = "test2@gmail.com",
                    CreatedDateTime = DateTime.UtcNow
                },
                new Template
                {
                    Name = "tests template 2",
                    Subject = "tests subject 2",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBodyTemplate = "test plain",
                    Priority = EmailMessagePriority.High,
                    Tag = "test tag)",
                    Language = "ru",
                    FromAddress = "test3@gmail.com",
                    CreatedDateTime = DateTime.UtcNow
                }
            };

            //Expect
            templatesRepository
                .Setup(x => x.GetQuerable())
                .Returns(new AsyncEnumerable<Template>(entities));

            //Act
            var filteredTemplate = await templatesManager.GetByName(searchingName);

            //Assert
            Assert.IsNotNull(filteredTemplate);

            Assert.AreEqual(entities[0].Name, filteredTemplate.Name);
            Assert.AreEqual(entities[0].Subject, filteredTemplate.Subject);
            Assert.AreEqual(entities[0].HtmlBodyTemplate, filteredTemplate.HtmlBodyTemplate);
            Assert.AreEqual(entities[0].PlainBodyTemplate, filteredTemplate.PlainBodyTemplate);
            Assert.AreEqual(Priority.Normal, filteredTemplate.Priority);
            Assert.AreEqual(entities[0].Tag, filteredTemplate.Tag);
            Assert.AreEqual(entities[0].Language, filteredTemplate.Language.Name);
            Assert.AreEqual(entities[0].FromAddress, filteredTemplate.FromAddress);

            //Verify
            templatesRepository.VerifyAll();
        }

        [Test]
        public async Task Create_Success()
        {
            //Arrange
            var templatesRepository = new Mock<ITemplatesRepository>();
            var templatesManager = new TemplatesManager(templatesRepository.Object);

            var expectedTemplate = new DefaultTemplate
            {
                Name = "test template",
                Subject = "test subject",
                HtmlBodyTemplate = "Hi, it's your email",
                PlainBodyTemplate = "test plain",
                Priority = Priority.Normal,
                Tag = "tag test",
                Language = CultureInfo.CurrentCulture,
                FromAddress = "test@gmail.com"
            };
            Template actualTemplate = null;

            //Expect
            templatesRepository
                .Setup(x => x.CreateAsync(It.IsNotNull<Template>()))
                .Callback<Template>(template =>
                {
                    actualTemplate = template;
                })
                .Returns(Task.CompletedTask);

            //Act
            await templatesManager.Create(expectedTemplate);

            //Assert
            Assert.IsNotNull(actualTemplate);
            Assert.AreEqual(expectedTemplate.Name, actualTemplate.Name);
            Assert.AreEqual(expectedTemplate.Subject, actualTemplate.Subject);
            Assert.AreEqual(expectedTemplate.HtmlBodyTemplate, actualTemplate.HtmlBodyTemplate);
            Assert.AreEqual(expectedTemplate.PlainBodyTemplate, actualTemplate.PlainBodyTemplate);
            Assert.AreEqual(EmailMessagePriority.Normal, actualTemplate.Priority);
            Assert.AreEqual(expectedTemplate.Tag, actualTemplate.Tag);
            Assert.AreEqual(expectedTemplate.Language.Name, actualTemplate.Language);
            Assert.AreEqual(expectedTemplate.FromAddress, actualTemplate.FromAddress);

            //Verify
            templatesRepository.VerifyAll();
        }

        [Test]
        public async Task Update_Success()
        {
            //Arrange
            var templatesRepository = new Mock<ITemplatesRepository>();
            var templatesManager = new TemplatesManager(templatesRepository.Object);

            var searchingTemplateName = "tests template 1";
            var entities = new List<Template>
            {
                new Template
                {
                    Name = searchingTemplateName,
                    Subject = "tests subject 1",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBodyTemplate = "test plain",
                    Priority = EmailMessagePriority.Normal,
                    Tag = "test tag",
                    Language = "en",
                    FromAddress = "test@gmail.com",
                    CreatedDateTime = DateTime.UtcNow
                },
                new Template
                {
                    Name = "tests template 2",
                    Subject = "tests subject 2",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBodyTemplate = "test plain",
                    Priority = EmailMessagePriority.Normal,
                    Tag = "test tag",
                    Language = "ru",
                    FromAddress = "test1@gmail.com",
                    CreatedDateTime = DateTime.UtcNow
                }
            };

            var templatesUpdates = new DefaultTemplate
            {
                Name = "test template)",
                Subject = "test subject",
                HtmlBodyTemplate = "Hi, it's your email",
                PlainBodyTemplate = "test plain",
                Priority = Priority.Low,
                Tag = "tag 1",
                Language = new CultureInfo("en"),
                FromAddress = "test2@gmail.com"
            };
            Template actualTemplateUpdates = null;

            //Expect
            templatesRepository
                .Setup(x => x.UpdateAsync(It.IsNotNull<Template>()))
                .Callback<Template>(template =>
                {
                    actualTemplateUpdates = template;
                })
                .Returns(Task.CompletedTask);

            templatesRepository
                .Setup(x => x.GetQuerable())
                .Returns(new AsyncEnumerable<Template>(entities));

            //Act
            await templatesManager.Update(searchingTemplateName, templatesUpdates);

            //Assert
            Assert.IsNotNull(actualTemplateUpdates);
            Assert.AreEqual(searchingTemplateName, actualTemplateUpdates.Name);
            Assert.AreEqual(templatesUpdates.Subject, actualTemplateUpdates.Subject);
            Assert.AreEqual(templatesUpdates.HtmlBodyTemplate, actualTemplateUpdates.HtmlBodyTemplate);
            Assert.AreEqual(templatesUpdates.PlainBodyTemplate, actualTemplateUpdates.PlainBodyTemplate);
            Assert.AreEqual(EmailMessagePriority.Low, actualTemplateUpdates.Priority);
            Assert.AreEqual(templatesUpdates.Tag, actualTemplateUpdates.Tag);
            Assert.AreEqual(templatesUpdates.Language.Name, actualTemplateUpdates.Language);
            Assert.AreEqual(templatesUpdates.FromAddress, actualTemplateUpdates.FromAddress);
            Assert.AreEqual(entities[0].CreatedDateTime, actualTemplateUpdates.CreatedDateTime);

            //Verify
            templatesRepository.VerifyAll();
        }

        [Test]
        public async Task Remove_Success()
        {
            //Arrange
            var templatesRepository = new Mock<ITemplatesRepository>();
            var templatesManager = new TemplatesManager(templatesRepository.Object);

            var searchingTemplateName = "tests template 2";
            var entities = new List<Template>
            {
                new Template
                {
                    Name = "tests template 1",
                    Subject = "tests subject 1",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBodyTemplate = "test plain",
                    Priority = EmailMessagePriority.Normal,
                    Tag = "test tag",
                    Language = "en",
                    FromAddress = "test1@gmail.com",
                    CreatedDateTime = DateTime.UtcNow
                },
                new Template
                {
                    Name = searchingTemplateName,
                    Subject = "tests subject 2",
                    HtmlBodyTemplate = "Hi my friend :)",
                    PlainBodyTemplate = "test plain",
                    Priority = EmailMessagePriority.Normal,
                    Tag = "test tag)",
                    Language = "en",
                    FromAddress = "test@gmail.com",
                    CreatedDateTime = DateTime.UtcNow
                }
            };
            Template removedTemplate = null;

            //Expect
            templatesRepository
                .Setup(x => x.GetQuerable())
                .Returns(new AsyncEnumerable<Template>(entities));

            templatesRepository
                .Setup(x => x.RemoveAsync(It.IsNotNull<Template>()))
                .Callback<Template>(template =>
                {
                    removedTemplate = template;
                })
                .Returns(Task.CompletedTask);

            //Act
            await templatesManager.Remove(searchingTemplateName);

            //Assert
            Assert.IsNotNull(removedTemplate);
            Assert.AreEqual(searchingTemplateName, removedTemplate.Name);
            Assert.AreEqual(entities[1].Subject, removedTemplate.Subject);
            Assert.AreEqual(entities[1].HtmlBodyTemplate, removedTemplate.HtmlBodyTemplate);
            Assert.AreEqual(entities[1].PlainBodyTemplate, removedTemplate.PlainBodyTemplate);
            Assert.AreEqual(entities[1].Priority, removedTemplate.Priority);
            Assert.AreEqual(entities[1].Tag, removedTemplate.Tag);
            Assert.AreEqual(entities[1].Language, removedTemplate.Language);
            Assert.AreEqual(entities[1].FromAddress, removedTemplate.FromAddress);
            Assert.AreEqual(entities[1].CreatedDateTime, removedTemplate.CreatedDateTime);

            //Verify
            templatesRepository.VerifyAll();
        }
    }
}

