using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentEmail.Source.Core.Interfaces
{
    public interface ITemplatesReader<TKey>
    {
        Task<IEnumerable<ITemplate<TKey>>> GetByLanguage(string language);

        Task<ITemplate<TKey>> GetById(TKey id);
    }
}
