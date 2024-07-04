using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.Requests
{
    public class GetWordRequest : IRequest<WordDto>
    {
        public int WordId { get; set; }
    }
}
