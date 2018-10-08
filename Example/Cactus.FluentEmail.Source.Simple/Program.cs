using System.Threading.Tasks;
using Cactus.FluentEmail.Source.Simple.Examples;

namespace Cactus.FluentEmail.Source.Simple
{
    class Program
    {
        static Program()
        {
            FluentEmailConfigurator.Configure();

            SenderExamples.Sender = "sender@gmail.com";
            SenderExamples.Recipient = "recipient@gmail.com";
        }

        static async Task Main(string[] args)
        {
            //examples of sending message
            await SenderExamples.UseBaseSend();
            await SenderExamples.SendHtmlModel();
            await SenderExamples.SendPlainModel();
            await SenderExamples.SendHtmlAndPlainModels();

            //examples of managing messages in a source
            await TemplatesManagerExamples.Create();
            await TemplatesManagerExamples.GetByName();
            await TemplatesManagerExamples.Update();
            await TemplatesManagerExamples.Remove();
        }
    }
}
