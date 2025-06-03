using Dictionary.Resources.Messages;
using FluentValidation;

namespace Dictionary.Domain.Validators
{
    public class TranslationGroupValidator : AbstractValidator<TranslationGroup>
    {
        public TranslationGroupValidator()
        {
            RuleFor(x => x.TranslationGroupDescriptionId)
                .NotEmpty()
                .WithMessage(x => string.Format(ValidationMessages.EitherMustHaveValue, nameof(x.TranslationGroupDescriptionId), nameof(x.TranslationGroupDescription)));
        }
    }
}
