using System.Linq;
using System.Threading.Tasks;
using Cactus.FluentEmail.Source.EntityFraemwork.Database;

namespace Cactus.FluentEmail.Source.EntityFraemwork.Repositories
{
    public interface ITemplatesRepository
    {
        IQueryable<Template> GetQuerable();

        Task CreateAsync(Template entity);
        Task UpdateAsync(Template entity);
        Task RemoveAsync(Template entity);
    }
}
