using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.Requests
{
    public class GetWordsRequest : IRequest<List<WordDto>>
    {
    }
}
