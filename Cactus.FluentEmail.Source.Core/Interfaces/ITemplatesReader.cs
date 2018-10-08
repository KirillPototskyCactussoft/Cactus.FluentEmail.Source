using System.Globalization;
using System.Threading.Tasks;

namespace Cactus.FluentEmail.Source.Core.Interfaces
{
    public interface ITemplatesReader
    {
        Task<ITemplate> GetByName(string name, CultureInfo language = null);
    }
}
