using AutoMapper;
using Dictionary.BusinessLogic.Abstractions.Tag;
using Dictionary.BusinessLogic.TranslationGroup.Requests;
using Dictionary.Data;
using Dictionary.Domain;
using Dictionary.Domain.Builders;
using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.TranslationGroup.Handlers
{
    public class PostTranslationGroupHandler : IRequestHandler<PostTranslationGroupRequest, TranslationGroupDto>
    {
        private readonly DictionaryContext dbContext;
        private readonly IMapper mapper;
        private readonly ITagService tagService;

        public PostTranslationGroupHandler(
            DictionaryContext dbContext,
            IMapper mapper,
            ITagService tagService)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        }

        public async Task<TranslationGroupDto> Handle(PostTranslationGroupRequest request, CancellationToken cancellationToken)
        {
            var tags = await tagService.GetOrCreateTagsAsync(request.TranslationGroup.Tags, cancellationToken);

            var newTranslationGroup = new TranslationGroupBuilder()
                .SetDescription(request.TranslationGroup.Description)
                .Build();

            var translationGroupTags = CreateTranslationGroupTags(newTranslationGroup, tags);

            await dbContext.TranslationGroups.AddAsync(newTranslationGroup, cancellationToken);
            await dbContext.TranslationGroupTags.AddRangeAsync(translationGroupTags, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<TranslationGroupDto>(newTranslationGroup);
        }

        private List<TranslationGroupTag> CreateTranslationGroupTags(Domain.TranslationGroup translationGroup, List<Domain.Tag> tags)
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
