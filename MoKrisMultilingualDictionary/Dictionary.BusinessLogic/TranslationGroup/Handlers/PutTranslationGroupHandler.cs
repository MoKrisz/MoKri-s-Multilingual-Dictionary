using AutoMapper;
using Dictionary.BusinessLogic.Abstractions.Services.Synchronization;
using Dictionary.BusinessLogic.Abstractions.Tag;
using Dictionary.BusinessLogic.Exceptions;
using Dictionary.BusinessLogic.Tag;
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
        private readonly IDataSynchronizer dataSynchronizer;
        private readonly ITagService tagService;

        public PutTranslationGroupHandler(
            DictionaryContext dbContext,
            IMapper mapper,
            IDataSynchronizer dataSynchronizer,
            ITagService tagService)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.dataSynchronizer = dataSynchronizer ?? throw new ArgumentNullException(nameof(dataSynchronizer));
            this.tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        }

        public async Task<TranslationGroupDto> Handle(PutTranslationGroupRequest request, CancellationToken cancellationToken)
        {
            var translationGroup = await dbContext.TranslationGroups
                .Include(tg => tg.TranslationGroupTags)
                    .ThenInclude(tgt => tgt.Tag)
                .SingleOrDefaultAsync(tg => tg.TranslationGroupId == request.TranslationGroup.TranslationGroupId, cancellationToken);

            if (translationGroup == null)
            {
                throw new NotFoundException(nameof(TranslationGroup));
            }

            translationGroup.GetBuilder()
                .SetDescription(request.TranslationGroup.Description)
                .Build();

            var tagSyncResult = dataSynchronizer.Synchronize(translationGroup.TranslationGroupTags.Select(tgt => tgt.Tag), request.TranslationGroup.Tags, new TagComparer());

            var addTags = await tagService.GetOrCreateTagsAsync([.. tagSyncResult.Added], cancellationToken);
            var addTranslationGroupTags = addTags.Select(tag =>
                new TranslationGroupTagBuilder()
                    .SetTranslationGroup(translationGroup)
                    .SetTag(tag)
                    .Build());

            var deleteTagIds = tagSyncResult.Deleted.Select(dt => dt.TagId);
            //var deleteTranslationGroupTags = translationGroup.TranslationGroupTags
            //    .Where(tgt => deleteTagIds.Contains(tgt.TagId))
            //    .ToList();

            translationGroup.TranslationGroupTags.RemoveAll(tgt => deleteTagIds.Contains(tgt.TagId));

            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<TranslationGroupDto>(translationGroup);
        }
    }
}
