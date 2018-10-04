using System;
using System.Globalization;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Source.Core.Interfaces;
using Moq;
using NUnit.Framework;

namespace FluentEmail.Source.Core.Tests
{
    [TestFixture]
    public class FluentEmailExtenssionTests
    {
        [Test]
        public void UseTemplate_Success()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            var template = new DefaultTemplate
            {
                Name = "tests template",
                Subject = "subject of template",
                HtmlBodyTemplate = "body of template",
                PlainBodyTemplate = "plain body of template",
                Priority = Priority.Normal,
                Tag = "tag of template",
                Language = CultureInfo.CurrentCulture,
                FromAddress = "test@gmail.com"
            };
            Func<ITemplate> getTemplate = () => template;
            var emailData = new EmailData();


            //Expect
            fluentEmail
                .Setup(x => x.Body(It.Is<string>(b => b == template.HtmlBodyTemplate), It.Is<bool>(i => i)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.PlaintextAlternativeBody(It.Is<string>(b => b == template.PlainBodyTemplate)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.Subject(It.Is<string>(b => b == template.Subject)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.SetFrom(It.Is<string>(b => b == template.FromAddress), It.Is<string>(n => n == null)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .SetupGet(x => x.Data)
                .Returns(emailData);

            fluentEmail
                .Setup(x => x.Tag(It.Is<string>(b => b == template.Tag)))
                .Returns(fluentEmail.Object);

            //Act
            FluentEmailExtenssion.UseTemplate(fluentEmail.Object, getTemplate);

            //Assert
            Assert.AreEqual(emailData.Priority, template.Priority);

            //Verify
            fluentEmail.VerifyAll();
        }

        [Test]
        public void UseTemplate_FuncGetTemplateIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplate(fluentEmail.Object, null));

            //Assert
            Assert.IsNotNull(exception);


            //Verify
            fluentEmail.Verify(x => x.Body(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeBody(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplate_TemplateIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            Func<ITemplate> getTemplate = () => null;

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplate(fluentEmail.Object, getTemplate));

            //Assert
            Assert.IsNotNull(exception);


            //Verify
            fluentEmail.Verify(x => x.Body(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeBody(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplateWithModels_Success()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            var template = new DefaultTemplate
            {
                Name = "tests template",
                Subject = "subject of template",
                HtmlBodyTemplate = "body of template",
                PlainBodyTemplate = "plain body of template",
                Priority = Priority.Normal,
                Tag = "tag of template",
                Language = CultureInfo.CurrentCulture,
                FromAddress = "test@gmail.com"
            };
            Func<ITemplate> getTemplate = () => template;
            var emailData = new EmailData();
            var htmlModel = new BodyModel
            {
                Name = "Kirill",
                Age = 22
            };
            var plainModel = new BodyModel
            {
                Name = "Vitalic",
                Age = 21
            };

            BodyModel actualHtmlModel = null;
            BodyModel actualPlainModel = null;

            //Expect
            fluentEmail
                .Setup(x => x.UsingTemplate(It.Is<string>(b => b == template.HtmlBodyTemplate), It.IsNotNull<BodyModel>(), It.Is<bool>(i => i)))
                .Callback<string, BodyModel, bool>((htmlTemplate, model, isHtml) =>
                {
                    actualHtmlModel = model;
                })
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.PlaintextAlternativeUsingTemplate(It.Is<string>(b => b == template.PlainBodyTemplate), It.IsNotNull<BodyModel>()))
                .Callback<string, BodyModel>((plainTemplate, model) =>
                {
                    actualPlainModel = model;
                })
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.Subject(It.Is<string>(b => b == template.Subject)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.SetFrom(It.Is<string>(b => b == template.FromAddress), It.Is<string>(n => n == null)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .SetupGet(x => x.Data)
                .Returns(emailData);

            fluentEmail
                .Setup(x => x.Tag(It.Is<string>(b => b == template.Tag)))
                .Returns(fluentEmail.Object);

            //Act
            FluentEmailExtenssion.UseTemplate(fluentEmail.Object, getTemplate, htmlModel, plainModel);

            //Assert
            Assert.AreEqual(emailData.Priority, template.Priority);

            Assert.IsNotNull(actualHtmlModel);
            Assert.AreEqual(htmlModel.Name, actualHtmlModel.Name);
            Assert.AreEqual(htmlModel.Age, actualHtmlModel.Age);

            Assert.IsNotNull(actualPlainModel);
            Assert.AreEqual(plainModel.Name, actualPlainModel.Name);
            Assert.AreEqual(plainModel.Age, actualPlainModel.Age);

            //Verify
            fluentEmail.VerifyAll();
        }

        [Test]
        public void UseTemplateWithModels_FuncGetTemplateIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplate(fluentEmail.Object, null, default(object), default(object)));

            //Assert
            Assert.IsNotNull(exception);

            //Verify
            fluentEmail.Verify(x => x.UsingTemplate(It.IsAny<string>(), Is.All, It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeUsingTemplate(It.IsAny<string>(), Is.All), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplateWithModels_TemplateIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            Func<ITemplate> getTemplate = () => null;

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplate(fluentEmail.Object, getTemplate, default(object), default(object)));

            //Assert
            Assert.IsNotNull(exception);

            //Verify
            fluentEmail.Verify(x => x.UsingTemplate(It.IsAny<string>(), Is.All, It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeUsingTemplate(It.IsAny<string>(), Is.All), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplateWithModels_HtmlModelIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            Func<ITemplate> getTemplate = () => new DefaultTemplate();

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplate(fluentEmail.Object, getTemplate, (object)null, default(object)));

            //Assert
            Assert.IsNotNull(exception);

            //Verify
            fluentEmail.Verify(x => x.UsingTemplate(It.IsAny<string>(), Is.All, It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeUsingTemplate(It.IsAny<string>(), Is.All), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplateWithModels_PlainModelIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            Func<ITemplate> getTemplate = () => new DefaultTemplate();

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplate(fluentEmail.Object, getTemplate, default(object), (object)null));

            //Assert
            Assert.IsNotNull(exception);

            //Verify
            fluentEmail.Verify(x => x.UsingTemplate(It.IsAny<string>(), Is.All, It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeUsingTemplate(It.IsAny<string>(), Is.All), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplateWithHtmlModel_Success()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            var template = new DefaultTemplate
            {
                Name = "tests template",
                Subject = "subject of template",
                HtmlBodyTemplate = "body of template",
                PlainBodyTemplate = "plain body of template",
                Priority = Priority.Normal,
                Tag = "tag of template",
                Language = CultureInfo.CurrentCulture,
                FromAddress = "test@gmail.com"
            };
            Func<ITemplate> getTemplate = () => template;
            var emailData = new EmailData();
            var htmlModel = new BodyModel
            {
                Name = "Kirill",
                Age = 22
            };

            BodyModel actualHtmlModel = null;

            //Expect
            fluentEmail
                .Setup(x => x.UsingTemplate(It.Is<string>(b => b == template.HtmlBodyTemplate), It.IsNotNull<BodyModel>(), It.Is<bool>(i => i)))
                .Callback<string, BodyModel, bool>((htmlTemplate, model, isHtml) =>
                {
                    actualHtmlModel = model;
                })
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.PlaintextAlternativeBody(It.Is<string>(b => b == template.PlainBodyTemplate)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.Subject(It.Is<string>(b => b == template.Subject)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.SetFrom(It.Is<string>(b => b == template.FromAddress), It.Is<string>(n => n == null)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .SetupGet(x => x.Data)
                .Returns(emailData);

            fluentEmail
                .Setup(x => x.Tag(It.Is<string>(b => b == template.Tag)))
                .Returns(fluentEmail.Object);

            //Act
            FluentEmailExtenssion.UseTemplateWithHtmlModel(fluentEmail.Object, getTemplate, htmlModel);

            //Assert
            Assert.AreEqual(emailData.Priority, template.Priority);

            Assert.IsNotNull(actualHtmlModel);
            Assert.AreEqual(htmlModel.Name, actualHtmlModel.Name);
            Assert.AreEqual(htmlModel.Age, actualHtmlModel.Age);

            //Verify
            fluentEmail.VerifyAll();
        }

        [Test]
        public void UseTemplateWithHtmlModel_FuncGetTemplateIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplateWithHtmlModel(fluentEmail.Object, null, default(object)));

            //Assert
            Assert.IsNotNull(exception);

            //Verify
            fluentEmail.Verify(x => x.UsingTemplate(It.IsAny<string>(), Is.All, It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeUsingTemplate(It.IsAny<string>(), Is.All), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplateWithHtmlModel_TemplateIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            Func<ITemplate> getTemplate = () => null;

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplateWithHtmlModel(fluentEmail.Object, getTemplate, default(object)));

            //Assert
            Assert.IsNotNull(exception);

            //Verify
            fluentEmail.Verify(x => x.UsingTemplate(It.IsAny<string>(), Is.All, It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeUsingTemplate(It.IsAny<string>(), Is.All), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplateWithHtmlModel_ModelIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            Func<ITemplate> getTemplate = () => new DefaultTemplate();

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplateWithHtmlModel(fluentEmail.Object, getTemplate, (object)null));

            //Assert
            Assert.IsNotNull(exception);

            //Verify
            fluentEmail.Verify(x => x.UsingTemplate(It.IsAny<string>(), Is.All, It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeUsingTemplate(It.IsAny<string>(), Is.All), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplateWithPlainModel_Success()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            var template = new DefaultTemplate
            {
                Name = "tests template",
                Subject = "subject of template",
                HtmlBodyTemplate = "body of template",
                PlainBodyTemplate = "plain body of template",
                Priority = Priority.Normal,
                Tag = "tag of template",
                Language = CultureInfo.CurrentCulture,
                FromAddress = "test@gmail.com"
            };
            Func<ITemplate> getTemplate = () => template;
            var emailData = new EmailData();
            var plainModel = new BodyModel
            {
                Name = "Vitalic",
                Age = 21
            };

            BodyModel actualPlainModel = null;

            //Expect
            fluentEmail
                .Setup(x => x.Body(It.Is<string>(b => b == template.HtmlBodyTemplate), It.Is<bool>(i => i)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.PlaintextAlternativeUsingTemplate(It.Is<string>(b => b == template.PlainBodyTemplate), It.IsNotNull<BodyModel>()))
                .Callback<string, BodyModel>((plainTemplate, model) =>
                {
                    actualPlainModel = model;
                })
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.Subject(It.Is<string>(b => b == template.Subject)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .Setup(x => x.SetFrom(It.Is<string>(b => b == template.FromAddress), It.Is<string>(n => n == null)))
                .Returns(fluentEmail.Object);

            fluentEmail
                .SetupGet(x => x.Data)
                .Returns(emailData);

            fluentEmail
                .Setup(x => x.Tag(It.Is<string>(b => b == template.Tag)))
                .Returns(fluentEmail.Object);

            //Act
            FluentEmailExtenssion.UseTemplateWithPlainModel(fluentEmail.Object, getTemplate, plainModel);

            //Assert
            Assert.AreEqual(emailData.Priority, template.Priority);

            Assert.IsNotNull(actualPlainModel);
            Assert.AreEqual(plainModel.Name, actualPlainModel.Name);
            Assert.AreEqual(plainModel.Age, actualPlainModel.Age);

            //Verify
            fluentEmail.VerifyAll();
        }

        [Test]
        public void UseTemplateWithPlainModel_FuncGetTemplateIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplateWithPlainModel(fluentEmail.Object, null, default(object)));

            //Assert
            Assert.IsNotNull(exception);

            //Verify
            fluentEmail.Verify(x => x.UsingTemplate(It.IsAny<string>(), Is.All, It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeUsingTemplate(It.IsAny<string>(), Is.All), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplateWithPlainModel_TemplateIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            Func<ITemplate> getTemplate = () => null;

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplateWithPlainModel(fluentEmail.Object, getTemplate, default(object)));

            //Assert
            Assert.IsNotNull(exception);

            //Verify
            fluentEmail.Verify(x => x.UsingTemplate(It.IsAny<string>(), Is.All, It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeUsingTemplate(It.IsAny<string>(), Is.All), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UseTemplateWithPlainModel_ModelIsNull_Fail()
        {
            //Arrange
            var fluentEmail = new Mock<IFluentEmail>();
            Func<ITemplate> getTemplate = () => new DefaultTemplate();

            //Expect

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => FluentEmailExtenssion.UseTemplateWithPlainModel(fluentEmail.Object, getTemplate, (object)null));

            //Assert
            Assert.IsNotNull(exception);

            //Verify
            fluentEmail.Verify(x => x.UsingTemplate(It.IsAny<string>(), Is.All, It.IsAny<bool>()), Times.Never);
            fluentEmail.Verify(x => x.PlaintextAlternativeUsingTemplate(It.IsAny<string>(), Is.All), Times.Never);
            fluentEmail.Verify(x => x.Subject(It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.SetFrom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fluentEmail.Verify(x => x.Tag(It.IsAny<string>()), Times.Never);
        }
    }
}
