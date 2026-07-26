using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.BusinessLogic.Services;
using Dictionary.Domain.Enums;
using Dictionary.Resources.Messages;
using FluentValidation;

namespace Dictionary.BusinessLogic.Practice.GuessArticle.Validators
{
    public class GetGuessArticleRandomWordsRequestValidator : AbstractValidator<GetGuessArticleRandomWordsRequest>
    {
        public const int MinWordAmount = 1;
        public const int MaxWordAmount = 20;

        public GetGuessArticleRandomWordsRequestValidator()
        {
            RuleFor(x => x.Amount)
                .InclusiveBetween(MinWordAmount, MaxWordAmount)
                .WithMessage(x => string.Format(ValidationMessages.MustBeBetweenValues, nameof(x.Amount), MinWordAmount, MaxWordAmount));

            RuleFor(x => x.LanguageCode)
                .Must(x => EnumService.TryConvertFromInt<LanguageCodeEnum>(x, out var _))
                .WithMessage(x => string.Format(ValidationMessages.InvalidValue, x, typeof(LanguageCodeEnum)));
        }
    }
}
