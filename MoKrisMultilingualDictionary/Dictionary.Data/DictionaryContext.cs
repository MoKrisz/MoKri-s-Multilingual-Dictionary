using Dictionary.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Data
{
    public class DictionaryContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TranslationGroup> TranslationGroups { get; set; }
        public DbSet<TranslationGroupTag> TranslationGroupTags { get; set; }
        public DbSet<WordTranslationGroup> WordTranslationGroups { get; set; }

        public DictionaryContext(DbContextOptions<DictionaryContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dictionary");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DictionaryContext).Assembly);
        }
    }
}
