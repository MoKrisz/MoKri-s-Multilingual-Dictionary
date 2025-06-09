using Dictionary.Domain;
using Dictionary.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations
{
    public class TranslationGroupConfiguration : IEntityTypeConfiguration<TranslationGroup>
    {
        public void Configure(EntityTypeBuilder<TranslationGroup> builder)
        {
            builder.Property(tg => tg.Description)
                .HasMaxLength(TranslationGroupConstants.DescriptionMaxLength)
                .IsRequired();

            builder.HasIndex(tg => tg.Description);

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
