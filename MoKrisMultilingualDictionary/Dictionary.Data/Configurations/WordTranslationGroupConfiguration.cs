using Dictionary.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations
{
    public class WordTranslationGroupConfiguration : IEntityTypeConfiguration<WordTranslationGroup>
    {
        public void Configure(EntityTypeBuilder<WordTranslationGroup> builder)
        {
            builder.HasOne(wtg => wtg.Word)
                .WithMany(w => w.WordTranslationGroups)
                .HasForeignKey(wtg => wtg.WordId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(wtg => wtg.TranslationGroup)
                .WithMany(tg => tg.WordTranslationGroups)
                .HasForeignKey(wtg => wtg.TranslationGroupId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
