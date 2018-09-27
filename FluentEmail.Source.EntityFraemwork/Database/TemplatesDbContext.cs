using Microsoft.EntityFrameworkCore;

namespace FluentEmail.Source.EntityFraemwork.Database
{
    public class TemplatesDbContext : DbContext
    {
        public TemplatesDbContext(DbContextOptions<TemplatesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Template

            modelBuilder.Entity<Template>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Template>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<Template>()
                .Property(x => x.Language)
                .IsRequired();

            modelBuilder.Entity<Template>()
                .HasIndex(x => new { x.Name, x.Language })
                .IsUnique();

            modelBuilder.Entity<Template>()
                .Property(x => x.Priority)
                .IsRequired();

            modelBuilder.Entity<Template>()
                .Property(x => x.IsHtml)
                .IsRequired();

            modelBuilder.Entity<Template>()
                .Property(x => x.Subject)
                .IsRequired(false);

            modelBuilder.Entity<Template>()
                .Property(x => x.HtmlBodyTemplate)
                .IsRequired(false);

            modelBuilder.Entity<Template>()
                .Property(x => x.PlainBodyTemplate)
                .IsRequired(false);

            modelBuilder.Entity<Template>()
                .Property(x => x.Tag)
                .IsRequired(false);

            modelBuilder.Entity<Template>()
                .Property(x => x.FromAddress)
                .IsRequired(false);

            modelBuilder.Entity<Template>()
                .Property(x => x.CreatedDateTime)
                .IsRequired();
            #endregion
        }

        public DbSet<Template> Templates { get; set; }
    }
}
