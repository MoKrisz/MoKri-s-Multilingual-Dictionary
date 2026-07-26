using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.Practice.GuessArticle.Requests
{
    public class EvaluateGuessArticleRequest : IRequest<List<EvaluateGuessArticleResponseItemDto>>
    {
        public List<EvaluateGuessArticleRequestItemDto> Guesses { get; set; } = new();
    }
}
