using AutoMapper;
using Dictionary.BusinessLogic.Exceptions;
using Dictionary.BusinessLogic.TranslationGroup.Requests;
using Dictionary.Data;
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
            var translationGroup = await dbContext.TranslationGroups
                .SingleOrDefaultAsync(tg => tg.TranslationGroupId == request.TranslationGroup.TranslationGroupId, cancellationToken);

            if (translationGroup == null)
            {
                throw new NotFoundException(nameof(TranslationGroup));
            }

            translationGroup.GetBuilder()
                .SetDescription(request.TranslationGroup.Description)
                .Build();

            var syncedTags = 
        }
    }
}
