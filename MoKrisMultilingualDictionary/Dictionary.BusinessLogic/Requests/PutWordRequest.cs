using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.Requests
{
    public class PutWordRequest : IRequest
    {
        public WordDto Word { get; set; }
    }
}
