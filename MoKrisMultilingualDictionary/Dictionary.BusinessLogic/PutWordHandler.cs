using Dictionary.BusinessLogic.Requests;
using Dictionary.Data;
using Dictionary.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.BusinessLogic
{
    public class PutWordHandler : IRequestHandler<PutWordRequest>
    {
        private readonly DictionaryContext dbContext;

        public PutWordHandler(DictionaryContext dbContext) 
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Handle(PutWordRequest request, CancellationToken cancellationToken)
        {
            var wordDto = request.Word;

            if (!Enum.TryParse<WordTypeEnum>(wordDto.Type.ToString(), out var wordType) ||
                !Enum.IsDefined(wordType))
            {
                throw new ArgumentOutOfRangeException(nameof(wordDto.Type), "Invalid word type.");
            };

            if (!Enum.TryParse<LanguageCodeEnum>(wordDto.LanguageCode.ToString(), out var language) ||
                !Enum.IsDefined(language))
            {
                throw new ArgumentOutOfRangeException(nameof(wordDto.LanguageCode), "Invalid language.");
            };

            var word = await dbContext.Words.SingleAsync(w => w.WordId == request.Word.WordId, cancellationToken);

            var wordBuilder = word.GetBuilder();

            wordBuilder.SetArticle(wordDto.Article)
                .SetText(wordDto.Text)
                .SetPlural(wordDto.Plural)
                .SetType(wordType)
                .SetConjugation(wordDto.Conjugation)
                .SetLanguageCode(language)
                .Build();

            await this.dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
