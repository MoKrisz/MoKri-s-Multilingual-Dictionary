using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.TranslationGroup.Requests
{
    public class GetWordRelatedTranslationGroupsRequest : IRequest<WordRelatedTranslationGroupsDto>
    {
        public int SourceWordId { get; set; } = default!;
        public int TargetWordId { get; set; } = default!;
    }
}
