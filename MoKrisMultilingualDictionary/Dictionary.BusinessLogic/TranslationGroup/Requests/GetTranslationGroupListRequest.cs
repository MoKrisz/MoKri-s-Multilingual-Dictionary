using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.TranslationGroup.Requests
{
    public class GetTranslationGroupListRequest : IRequest<List<TranslationGroupDto>>
    {
    }
}
