using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.Requests
{
    public class GetClosestMatchingTagRequest : IRequest<TagDto?>
    {
        public string TagText { get; set; } = string.Empty;
    }
}
