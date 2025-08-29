using AutoMapper;
using Dictionary.BusinessLogic.TranslationGroup.Requests;
using Dictionary.Data;
using Dictionary.Domain;
using Dictionary.Domain.Builders;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic.TranslationGroup.Handlers
{
    public class PutTranslationGroupHandler : IRequestHandler<PutTranslationGroupRequest, TranslationGroupDto>
    {
        private readonly DictionaryContext dbContext;
        private readonly IMapper mapper;

        public PutTranslationGroupHandler(DictionaryContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TranslationGroupDto> Handle(PutTranslationGroupRequest request, CancellationToken cancellationToken)
        {
            var tags = await GetTagsAsync(request.TranslationGroup.Tags, cancellationToken);

            var newTranslationGroup = new TranslationGroupBuilder()
                .SetDescription(request.TranslationGroup.Description)
                .Build();

            var translationGroupTags = CreateTranslationGroupTags(newTranslationGroup, tags);

            await dbContext.TranslationGroups.AddAsync(newTranslationGroup, cancellationToken);
            await dbContext.TranslationGroupTags.AddRangeAsync(translationGroupTags, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<TranslationGroupDto>(newTranslationGroup);
        }

        private async Task<List<Tag>> GetTagsAsync(List<TagDto> tags, CancellationToken cancellationToken)
        {
            var newTags = tags
                .Where(t => t.TagId == null)
                .Select(tag => 
                    new TagBuilder()
                        .SetText(tag.Text.Trim().ToLowerInvariant())
                        .Build());

            var existingTagIds = tags.Where(t => t.TagId != null).Select(t => t.TagId!.Value);
            var existingTags = await dbContext.Tags
                .Where(t => existingTagIds.Contains(t.TagId))
                .ToListAsync(cancellationToken);

            return [.. newTags, .. existingTags];
        }

        private List<TranslationGroupTag> CreateTranslationGroupTags(Domain.TranslationGroup translationGroup, List<Tag> tags)
        {
            return tags.Select(tag =>
                new TranslationGroupTagBuilder()
                    .SetTranslationGroup(translationGroup)
                    .SetTag(tag)
                    .Build())
                .ToList();
        }
    }
}
