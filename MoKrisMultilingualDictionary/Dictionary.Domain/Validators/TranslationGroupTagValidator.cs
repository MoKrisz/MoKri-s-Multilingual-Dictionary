using Dictionary.Resources.Messages;
using FluentValidation;

namespace Dictionary.Domain.Validators
{
    public class TranslationGroupTagValidator : AbstractValidator<TranslationGroupTag>
    {
        public TranslationGroupTagValidator()
        {
            RuleFor(x => x.TranslationGroupId)
                .NotEmpty()
                .When(x => x.TranslationGroup == null)
                .WithMessage(x => string.Format(ValidationMessages.EitherMustHaveValue, nameof(x.TranslationGroupId), nameof(x.TranslationGroup)));

            RuleFor(x => x.TagId)
                .NotEmpty()
                .When(x => x.Tag == null)
                .WithMessage(x => string.Format(ValidationMessages.EitherMustHaveValue, nameof(x.TagId), nameof(x.Tag)));
        }
    }
}
