using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.TranslationGroup.Requests
{
    public class GetTranslationGroupsRequest : IRequest<List<TranslationGroupDto>>
    {
    }
}
