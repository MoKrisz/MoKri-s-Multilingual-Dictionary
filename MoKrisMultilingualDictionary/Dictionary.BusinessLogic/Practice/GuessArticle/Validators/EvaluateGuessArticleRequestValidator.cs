using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.Resources.Messages;
using FluentValidation;

namespace Dictionary.BusinessLogic.Practice.GuessArticle.Validators
{
    public class EvaluateGuessArticleRequestValidator : AbstractValidator<EvaluateGuessArticleRequest>
    {
        public const int MaxWordAmount = 20;

        public EvaluateGuessArticleRequestValidator()
        {
            RuleFor(x => x.Guesses)
                .Must(g => g.Count <= MaxWordAmount)
                .WithMessage(x => string.Format(ValidationMessages.MaximumValue, $"Number of {nameof(x.Guesses)}", MaxWordAmount));
        }
    }
}
