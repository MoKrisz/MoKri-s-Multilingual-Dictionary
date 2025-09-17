using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.TranslationGroup.Requests
{
    public class PutTranslationGroupRequest : IRequest<TranslationGroupDto>
    {
        public TranslationGroupDto TranslationGroup { get; set; } = default!;
    }
}
