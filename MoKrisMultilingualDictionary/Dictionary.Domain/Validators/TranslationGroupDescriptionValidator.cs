using Dictionary.Domain.Constants;
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
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Description)))
                //
                .MaximumLength(TranslationGroupDescriptionConstants.DescriptionMaxLength)
                .WithMessage(x => string.Format(ValidationMessages.MaximumLength, nameof(x.Description), TranslationGroupDescriptionConstants.DescriptionMaxLength));

            RuleFor(x => x.TranslationGroupId)
                .NotEmpty()
                .When(x => x.TranslationGroup == null)
                .WithMessage(x => string.Format(ValidationMessages.EitherMustHaveValue, nameof(x.TranslationGroupId), nameof(x.TranslationGroup)));
        }
    }
}
