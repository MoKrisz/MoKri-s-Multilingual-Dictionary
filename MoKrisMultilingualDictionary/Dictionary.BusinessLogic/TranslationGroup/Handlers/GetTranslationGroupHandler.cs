using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dictionary.BusinessLogic.Exceptions;
using Dictionary.BusinessLogic.TranslationGroup.Requests;
using Dictionary.Data;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic.TranslationGroup.Handlers
{
    public class GetTranslationGroupHandler : IRequestHandler<GetTranslationGroupRequest, TranslationGroupDto>
    {
        private readonly DictionaryContext dbContext;
        private readonly IMapper mapper;

        public GetTranslationGroupHandler(DictionaryContext dbContext,
            IMapper mapper) 
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TranslationGroupDto> Handle(GetTranslationGroupRequest request, CancellationToken cancellationToken)
        {
            var translationGroup = await dbContext.TranslationGroups
                .AsNoTracking()
                .ProjectTo<TranslationGroupDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(tg => tg.TranslationGroupId == request.TranslationGroupId, cancellationToken);

            if (translationGroup == null)
            {
                throw new NotFoundException(nameof(TranslationGroup));
            }

            return translationGroup;
        }
    }
}
