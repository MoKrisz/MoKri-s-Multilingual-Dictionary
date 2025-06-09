using Dictionary.Domain.Constants;
using Dictionary.Resources.Messages;
using FluentValidation;

namespace Dictionary.Domain.Validators
{
    public class TagValidator : AbstractValidator<Tag>
    {
        public TagValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Text)))
                //
                .MaximumLength(TagConstants.TextMaxLength)
                .WithMessage(x => string.Format(ValidationMessages.MaximumLength, nameof(x.Text), TagConstants.TextMaxLength));
        }
    }
}
