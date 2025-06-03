using Dictionary.Resources.Messages;
using FluentValidation;

namespace Dictionary.Domain.Validators
{
    public class WordTranslationGroupValidator : AbstractValidator<WordTranslationGroup>
    {
        public WordTranslationGroupValidator()
        {
            RuleFor(x => x.WordId)
                .NotEmpty()
                .When(x => x.Word == null)
                .WithMessage(x => string.Format(ValidationMessages.EitherMustHaveValue, nameof(x.WordId), nameof(x.Word)));

            RuleFor(x => x.TranslationGroupId)
                .NotEmpty()
                .When(x => x.TranslationGroup == null)
                .WithMessage(x => string.Format(ValidationMessages.EitherMustHaveValue, nameof(x.TranslationGroupId), nameof(x.TranslationGroup)));
        }
    }
}
