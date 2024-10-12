using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.Requests
{
    public class PostWordRequest : IRequest<int>
    {
        public WordDto Word { get; set; }
    }
}
