using Dictionary.BusinessLogic.Requests;
using Dictionary.Data;
using Dictionary.Models;
using Dictionary.Models.Enums;
using MediatR;

namespace Dictionary.BusinessLogic
{
    public class PostWordHandler : IRequestHandler<PostWordRequest, int>
    {
        private readonly DictionaryContext dbContext;

        public PostWordHandler(DictionaryContext dbContext) 
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> Handle(PostWordRequest request, CancellationToken cancellationToken)
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

            var wordBuilder = new Word().GetBuilder();

            var word = wordBuilder.SetArticle(wordDto.Article)
                .SetText(wordDto.Text)
                .SetPlural(wordDto.Plural)
                .SetType(wordType)
                .SetConjugation(wordDto.Conjugation)
                .SetLanguageCode(language)
                .Build();

            await this.dbContext.Words.AddAsync(word, cancellationToken);
            await this.dbContext.SaveChangesAsync(cancellationToken);

            return word.WordId;
        }
    }
}
