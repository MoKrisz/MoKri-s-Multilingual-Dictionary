using AutoMapper;
using Dictionary.BusinessLogic.Exceptions;
using Dictionary.BusinessLogic.Requests;
using Dictionary.Data;
using Dictionary.Models;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic
{
    public class GetWordHandler : IRequestHandler<GetWordRequest, WordDto>
    {
        private readonly DictionaryContext dbContext;
        private readonly IMapper mapper;

        public GetWordHandler(DictionaryContext dbContext,
            IMapper mapper) 
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<WordDto> Handle(GetWordRequest request, CancellationToken cancellationToken)
        {
            var word = await dbContext.Words
                .SingleOrDefaultAsync(w => w.WordId == request.WordId, cancellationToken);

            if (word == null)
            {
                throw new NotFoundException(nameof(Word));
            }

            return mapper.Map<WordDto>(word);
        }
    }
}
