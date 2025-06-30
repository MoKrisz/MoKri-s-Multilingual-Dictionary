using Dictionary.BusinessLogic.TranslationGroup.Requests;
using Dictionary.Data;
using Dictionary.Domain;
using Dictionary.Domain.Builders;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic.TranslationGroup.Handlers
{
    public class PostTranslationGroupHandler : IRequestHandler<PostTranslationGroupRequest, int>
    {
        private readonly DictionaryContext dbContext;

        public PostTranslationGroupHandler(DictionaryContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> Handle(PostTranslationGroupRequest request, CancellationToken cancellationToken)
        {
            var tags = await GetTagsAsync(request.TranslationGroup.Tags, cancellationToken);

            var newTranslationGroup = new TranslationGroupBuilder()
                .SetDescription(request.TranslationGroup.Description)
                .Build();

            var translationGroupTags = CreateTranslationGroupTags(newTranslationGroup, tags);

            await dbContext.TranslationGroups.AddAsync(newTranslationGroup, cancellationToken);
            await dbContext.TranslationGroupTags.AddRangeAsync(translationGroupTags, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return newTranslationGroup.TranslationGroupId;
        }

        private async Task<List<Tag>> GetTagsAsync(List<TagDto> tags, CancellationToken cancellationToken)
        {
            var newTagTexts = tags.Where(t => t.TagId == null).Select(t => t.Text);

            var newTags = new List<Tag>();
            foreach (var newTagText in newTagTexts)
            {
                var newTag = new TagBuilder().SetText(newTagText).Build();
                newTags.Add(newTag);
            }

            var existingTagIds = tags.Where(t => t.TagId != null).Select(t => t.TagId);
            var existingTags = await dbContext.Tags
                .Where(t => existingTagIds.Contains(t.TagId))
                .ToListAsync(cancellationToken);

            return newTags.Union(existingTags).ToList();
        }

        private List<TranslationGroupTag> CreateTranslationGroupTags(Domain.TranslationGroup translationGroup, List<Tag> tags)
        {
            var translationGroupTags = new List<TranslationGroupTag>();
            foreach (var tag in tags)
            {
                var newTranslationGroupTag = new TranslationGroupTagBuilder()
                    .SetTranslationGroup(translationGroup)
                    .SetTag(tag)
                    .Build();

                translationGroupTags.Add(newTranslationGroupTag);
            }

            return translationGroupTags;
        }
    }
}
