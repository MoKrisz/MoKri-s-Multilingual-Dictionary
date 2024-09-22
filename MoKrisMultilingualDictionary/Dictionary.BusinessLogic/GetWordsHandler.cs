using AutoMapper;
using Dictionary.BusinessLogic.Requests;
using Dictionary.Data;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic
{
    public class GetWordsHandler : IRequestHandler<GetWordsRequest, List<WordDto>>
    {
        private readonly DictionaryContext dbContext;
        private readonly IMapper mapper;

        public GetWordsHandler(DictionaryContext dbContext,
            IMapper mapper) 
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<WordDto>> Handle(GetWordsRequest request, CancellationToken cancellationToken)
        {
            var words = await dbContext.Words.Select(w => mapper.Map<WordDto>(w))
                .ToListAsync(cancellationToken);

            return words;
        }
    }
}
