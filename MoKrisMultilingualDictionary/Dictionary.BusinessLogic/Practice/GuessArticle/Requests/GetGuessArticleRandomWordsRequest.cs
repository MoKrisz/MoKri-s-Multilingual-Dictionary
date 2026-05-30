using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.Practice.GuessArticle.Requests
{
    public class GetGuessArticleRandomWordsRequest : IRequest<List<GuessArticleWordDto>>
    {
        public int LanguageCode { get; set; }
        public int Amount { get; set; }
    }
}
