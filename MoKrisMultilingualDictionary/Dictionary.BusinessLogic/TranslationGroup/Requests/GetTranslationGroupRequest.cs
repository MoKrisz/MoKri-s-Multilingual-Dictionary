using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.TranslationGroup.Requests
{
    public class GetTranslationGroupRequest : IRequest<TranslationGroupDto>
    {
        public int TranslationGroupId { get; set; }
    }
}
