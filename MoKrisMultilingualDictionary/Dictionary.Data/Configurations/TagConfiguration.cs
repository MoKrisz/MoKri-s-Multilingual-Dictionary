using Dictionary.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(t => t.Text)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(t => t.Text).IsUnique();

            builder.HasMany(t => t.TranslationGroupTags)
                .WithOne(tgt => tgt.Tag)
                .HasForeignKey(tgt => tgt.TagId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
