using Dictionary.BusinessLogic.Abstractions.Tag;
using Dictionary.Data;
using Dictionary.Domain.Builders;
using Dictionary.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic.Tag.Services
{
    public class TagService : ITagService
    {
        private readonly DictionaryContext dbContext;

        public TagService(DictionaryContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<Domain.Tag>> GetOrCreateTagsAsync(List<TagDto> tagDtos, CancellationToken cancellationToken)
        {
            var ids = tagDtos.Select(t => t.TagId).ToList();
            var normalizedTexts = tagDtos.Select(t => t.Text.ToLower()).ToList();

            var existingTags = await dbContext.Tags
                .Where(t => ids.Contains(t.TagId) || normalizedTexts.Contains(t.Text.ToLower()))
                .ToListAsync(cancellationToken);

            var newTags = tagDtos
                .Where(t => !existingTags.Any(et => et.TagId == t.TagId 
                                                    || et.Text.Equals(t.Text, StringComparison.InvariantCultureIgnoreCase)))
                .Select(tag =>
                    new TagBuilder()
                        .SetText(tag.Text.Trim().ToLowerInvariant())
                        .Build());

            return [.. newTags, .. existingTags];
        }
    }
}
