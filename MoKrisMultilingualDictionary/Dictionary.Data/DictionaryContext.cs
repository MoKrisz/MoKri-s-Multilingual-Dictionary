using Dictionary.Models;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Data
{
    public class DictionaryContext : DbContext
    {
        public DbSet<Word> Words { get; set; }

        public DictionaryContext(DbContextOptions<DictionaryContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dictionary");
        }
    }
}
