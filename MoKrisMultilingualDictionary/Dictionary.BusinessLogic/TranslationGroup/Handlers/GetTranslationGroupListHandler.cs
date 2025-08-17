using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dictionary.BusinessLogic.TranslationGroup.Requests;
using Dictionary.Data;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic.TranslationGroup.Handlers
{
    public class GetTranslationGroupListHandler : IRequestHandler<GetTranslationGroupListRequest, List<TranslationGroupDto>>
    {
        private readonly DictionaryContext dbContext;
        private readonly IMapper mapper;

        public GetTranslationGroupListHandler(DictionaryContext dbContext,
            IMapper mapper) 
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<TranslationGroupDto>> Handle(GetTranslationGroupListRequest request, CancellationToken cancellationToken)
        {
            var translationGroups = await dbContext.TranslationGroups
                .AsNoTracking()
                .ProjectTo<TranslationGroupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return translationGroups;
        }
    }
}
