using Dictionary.Domain.Constants;
using Dictionary.Resources.Messages;
using FluentValidation;

namespace Dictionary.Domain.Validators
{
    public class TranslationGroupValidator : AbstractValidator<TranslationGroup>
    {
        public TranslationGroupValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Description)))
                //
                .MaximumLength(TranslationGroupConstants.DescriptionMaxLength)
                .WithMessage(x => string.Format(ValidationMessages.MaximumLength, nameof(x.Description), TranslationGroupConstants.DescriptionMaxLength));
        }
    }
}
