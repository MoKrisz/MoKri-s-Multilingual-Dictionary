using Dictionary.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations
{
    public class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.Property(w => w.Text)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(w => w.Type).IsRequired();

            builder.Property(w => w.LanguageCode).IsRequired();

            builder.Property(w => w.Article).HasMaxLength(5);

            builder.Property(w => w.Plural).HasMaxLength(200);

            builder.Property(w => w.Conjugation).HasMaxLength(500);

            //TODO: think about other possible indexes
            builder.HasIndex(w => w.Text);

            builder.HasMany(w => w.WordTranslationGroups)
                .WithOne(wtg => wtg.Word)
                .HasForeignKey(wtg => wtg.WordId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
