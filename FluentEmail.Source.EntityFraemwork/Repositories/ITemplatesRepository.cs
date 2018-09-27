using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Source.EntityFraemwork.Database;

namespace FluentEmail.Source.EntityFraemwork.Repositories
{
    public interface ITemplatesRepository
    {
        IQueryable<Template> GetQuerable();

        Task CreateAsync(Template entity);
        Task UpdateAsync(Template entity);
        Task RemoveAsync(Template entity);
    }
}
