using AutoMapper;
using Dictionary.BusinessLogic.Exceptions;
using Dictionary.BusinessLogic.Requests;
using Dictionary.Data;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic.Handlers.Tag
{
    public class GetClosestMatchingTagHandler : IRequestHandler<GetClosestMatchingTagRequest, TagDto?>
    {
        private readonly DictionaryContext dbContext;
        private readonly IMapper mapper;

        public GetClosestMatchingTagHandler(DictionaryContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TagDto?> Handle(GetClosestMatchingTagRequest request, CancellationToken cancellationToken)
        {
            var tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Text.ToLower().StartsWith(request.TagText.ToLower()), cancellationToken);

            return tag != null ? mapper.Map<TagDto>(tag) : null;
        }
    }
}
