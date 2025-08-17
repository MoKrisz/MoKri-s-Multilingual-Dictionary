using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.Translation.Requests
{
    public class PostTranslationRequest : IRequest
    {
        public int SourceWordId { get; set; }
        public int TargetWordId { get; set; }
        public List<int> LinkedTranslationGroupIds { get; set; } = new();
    }
}
