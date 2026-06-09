using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.Data;
using Dictionary.Models.Dtos;
using MediatR;

namespace Dictionary.BusinessLogic.Practice.GuessArticle.Handlers
{
    public class EvaluateGuessArticleHandler : IRequestHandler<EvaluateGuessArticleRequest, List<EvaluateGuessArticleResponseItemDto>>
    {
        private readonly DictionaryContext dbContext;

        public EvaluateGuessArticleHandler(DictionaryContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<List<EvaluateGuessArticleResponseItemDto>> Handle(EvaluateGuessArticleRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
