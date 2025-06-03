using Dictionary.Resources.Messages;
using FluentValidation;

namespace Dictionary.Domain.Validators
{
    public class TranslationGroupDescriptionValidator : AbstractValidator<TranslationGroupDescription>
    {
        public TranslationGroupDescriptionValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Description)));
        }
    }
}
