using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.Data;
using Dictionary.Domain.Rules;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic.Practice.GuessArticle.Handlers
{
    public class EvaluateGuessArticleHandler : IRequestHandler<EvaluateGuessArticleRequest, List<EvaluateGuessArticleResponseItemDto>>
    {
        private readonly DictionaryContext dbContext;

        public EvaluateGuessArticleHandler(DictionaryContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<EvaluateGuessArticleResponseItemDto>> Handle(EvaluateGuessArticleRequest request, CancellationToken cancellationToken)
        {
            var wordIds = request.Guesses.Select(g => g.WordId).ToList();

            var languagesWithArticles = WordArticleRules.ValidArticlesByLanguage.Keys.ToList();

            var words = await dbContext.Words
                .AsNoTracking()
                .Where(w => wordIds.Contains(w.WordId)
                    && w.Type == Domain.Enums.WordTypeEnum.Noun
                    && languagesWithArticles.Contains(w.LanguageCode))
                .ToListAsync(cancellationToken);

            var response = new List<EvaluateGuessArticleResponseItemDto>();

            foreach (var guess in request.Guesses)
            {
                var relatedWord = words.FirstOrDefault(w => w.WordId == guess.WordId);

                if (relatedWord != null)
                {
                    response.Add(new EvaluateGuessArticleResponseItemDto 
                    {
                        WordId = relatedWord.WordId,
                        Text = relatedWord.Text,
                        Answer = guess.Answer,
                        CorrectArticle = relatedWord.Article!,
                        IsCorrect = relatedWord.Article == guess.Answer
                    });
                }
            }

            return response;
        }
    }
}
