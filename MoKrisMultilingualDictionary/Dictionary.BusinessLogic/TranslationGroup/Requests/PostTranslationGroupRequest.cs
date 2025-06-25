using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.TranslationGroup.Requests
{
    public class PostTranslationGroupRequest : IRequest<int>
    {
        public TranslationGroupDto TranslationGroup { get; set; } = default!;
    }
}
