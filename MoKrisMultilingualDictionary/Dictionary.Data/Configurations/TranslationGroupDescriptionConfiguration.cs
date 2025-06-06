using Dictionary.Domain;
using Dictionary.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations
{
    public class TranslationGroupDescriptionConfiguration : IEntityTypeConfiguration<TranslationGroupDescription>
    {
        public void Configure(EntityTypeBuilder<TranslationGroupDescription> builder)
        {
            builder.Property(tgd => tgd.Description)
                .HasMaxLength(TranslationGroupDescriptionConstants.DescriptionMaxLength)
                .IsRequired();

            builder.HasIndex(tgd => tgd.Description);

            builder.HasOne(tgd => tgd.TranslationGroup)
                .WithOne(tg => tg.TranslationGroupDescription)
                .HasForeignKey<TranslationGroupDescription>(tgd => tgd.TranslationGroupId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
