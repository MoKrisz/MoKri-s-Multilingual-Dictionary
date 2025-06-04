using Dictionary.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations
{
    public class TranslationGroupConfiguration : IEntityTypeConfiguration<TranslationGroup>
    {
        public void Configure(EntityTypeBuilder<TranslationGroup> builder)
        {
            builder.HasOne(tg => tg.TranslationGroupDescription)
                .WithOne(tgd => tgd.TranslationGroup)
                .HasForeignKey<TranslationGroup>(tg => tg.TranslationGroupDescriptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(tg => tg.TranslationGroupTags)
                .WithOne(tgt => tgt.TranslationGroup)
                .HasForeignKey(tgt => tgt.TranslationGroupId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(tg => tg.WordTranslationGroups)
                .WithOne(wtg => wtg.TranslationGroup)
                .HasForeignKey(wtg => wtg.TranslationGroupId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
