using System.Threading.Tasks;

namespace Cactus.FluentEmail.Source.Core.Interfaces
{
    public interface ITemplatesWriter
    {
        Task Create(ITemplate template);
        Task Update(string name, ITemplate templateUpdates);
        Task Remove(string name);
    }
}
