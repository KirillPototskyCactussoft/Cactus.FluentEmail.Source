using System.Linq;
using System.Threading.Tasks;
using Cactus.FluentEmail.Source.EntityFraemwork.Database;

namespace Cactus.FluentEmail.Source.EntityFraemwork.Repositories
{
    public class TemplatesRepository : ITemplatesRepository
    {
        private readonly TemplatesDbContext _context;

        public TemplatesRepository(TemplatesDbContext context)
        {
            _context = context;
        }

        public IQueryable<Template> GetQuerable()
        {
            return _context.Set<Template>();
        }

        public async Task CreateAsync(Template entity)
        {
            await _context.Set<Template>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Template entity)
        {
            _context.Templates.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Template entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
