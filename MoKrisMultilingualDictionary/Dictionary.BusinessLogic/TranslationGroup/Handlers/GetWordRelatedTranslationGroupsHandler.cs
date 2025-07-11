using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dictionary.BusinessLogic.TranslationGroup.Requests;
using Dictionary.Data;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic.TranslationGroup.Handlers
{
    public class GetWordRelatedTranslationGroupsHandler : IRequestHandler<GetWordRelatedTranslationGroupsRequest, WordRelatedTranslationGroupsDto>
    {
        private readonly DictionaryContext dbContext;
        private readonly IMapper mapper;

        public GetWordRelatedTranslationGroupsHandler(DictionaryContext dbContext,
            IMapper mapper) 
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<WordRelatedTranslationGroupsDto> Handle(GetWordRelatedTranslationGroupsRequest request, CancellationToken cancellationToken)
        {
            var translationGroups = await dbContext.TranslationGroups
                .AsNoTracking()
                .Include(tg => tg.WordTranslationGroups)
                .Where(tg => tg.WordTranslationGroups.Any(wtg => wtg.WordId == request.SourceWordId || wtg.WordId == request.TargetWordId))
                .ToListAsync(cancellationToken);


            var linkedTranslationGroups = translationGroups.Where(tg =>
                tg.WordTranslationGroups.Any(wtg => wtg.WordId == request.SourceWordId)
                && tg.WordTranslationGroups.Any(wtg => wtg.WordId == request.TargetWordId));

            var potentialTranslationGroups = translationGroups.Except(linkedTranslationGroups);

            return new WordRelatedTranslationGroupsDto
            {
                PotentialTranslationGroups = potentialTranslationGroups.Select(mapper.Map<TranslationGroupDto>).ToList(),
                LinkedTranslationGroups = linkedTranslationGroups.Select(mapper.Map<TranslationGroupDto>).ToList()
            };
        }
    }
}
