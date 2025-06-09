using Dictionary.Domain;
using Dictionary.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations
{
    public class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.Property(w => w.Text)
                .HasMaxLength(WordConstants.TextMaxLength)
                .IsRequired();

            builder.Property(w => w.Type).IsRequired();

            builder.Property(w => w.LanguageCode).IsRequired();

            builder.Property(w => w.Article).HasMaxLength(WordConstants.ArticleMaxLength);

            builder.Property(w => w.Plural).HasMaxLength(WordConstants.PluralMaxLength);

            builder.Property(w => w.Conjugation).HasMaxLength(WordConstants.ConjugationMaxLength);

            //TODO: think about other possible indexes
            builder.HasIndex(w => w.Text);

            builder.HasMany(w => w.WordTranslationGroups)
                .WithOne(wtg => wtg.Word)
                .HasForeignKey(wtg => wtg.WordId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
