using Dictionary.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations
{
    public class TranslationGroupTagConfiguration : IEntityTypeConfiguration<TranslationGroupTag>
    {
        public void Configure(EntityTypeBuilder<TranslationGroupTag> builder)
        {
            builder.HasOne(tgt => tgt.TranslationGroup)
                .WithMany(tg => tg.TranslationGroupTags)
                .HasForeignKey(tgt => tgt.TranslationGroupId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(tgt => tgt.Tag)
                .WithMany(t => t.TranslationGroupTags)
                .HasForeignKey(tgt => tgt.TagId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
