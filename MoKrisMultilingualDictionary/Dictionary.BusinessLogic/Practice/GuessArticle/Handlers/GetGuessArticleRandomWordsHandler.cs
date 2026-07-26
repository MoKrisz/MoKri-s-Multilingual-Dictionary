using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.Data;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic.Practice.GuessArticle.Handlers
{
    public class GetGuessArticleRandomWordsHandler : IRequestHandler<GetGuessArticleRandomWordsRequest, List<GuessArticleWordDto>>
    {
        private readonly DictionaryContext dbContext;

        public GetGuessArticleRandomWordsHandler(DictionaryContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<GuessArticleWordDto>> Handle(GetGuessArticleRandomWordsRequest request, CancellationToken cancellationToken)
        {
            var randomWords = await dbContext.Words
                .AsNoTracking()
                .Where(w => (int)w.LanguageCode == request.LanguageCode
                    && w.Type == Domain.Enums.WordTypeEnum.Noun)
                .OrderBy(w => EF.Functions.Random())
                .Take(request.Amount)
                .Select(w => new GuessArticleWordDto
                {
                    WordId = w.WordId,
                    Text = w.Text
                })
                .ToListAsync(cancellationToken);

            return randomWords;
        }
    }
}
