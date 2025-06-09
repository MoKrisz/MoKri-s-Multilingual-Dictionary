using Dictionary.Domain.Constants;
using Dictionary.Domain.Enums;
using Dictionary.Resources.Messages;
using FluentValidation;

namespace Dictionary.Domain.Validators
{
    public class WordValidator : AbstractValidator<Word>
    {
        public WordValidator()
        {
            RuleFor(x => x.Article)
                .Empty()
                .When(x => x.LanguageCode != LanguageCodeEnum.DE 
                    || x.Type != WordTypeEnum.Noun, ApplyConditionTo.CurrentValidator)
                .WithMessage(ValidationMessages.ArticleMustBeEmpty)
                //
                .NotEmpty()
                .When(x => x.LanguageCode == LanguageCodeEnum.DE 
                    && x.Type == WordTypeEnum.Noun, ApplyConditionTo.CurrentValidator)
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Article)))
                //
                .MaximumLength(WordConstants.ArticleMaxLength)
                .WithMessage(x => string.Format(ValidationMessages.MaximumLength, nameof(x.Article), WordConstants.ArticleMaxLength));

            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Text)))
                //
                .MaximumLength(WordConstants.TextMaxLength)
                .WithMessage(x => string.Format(ValidationMessages.MaximumLength, nameof(x.Text), WordConstants.TextMaxLength));

            RuleFor(x => x.Plural)
                .Empty()
                .When(x => x.Type != WordTypeEnum.Noun, ApplyConditionTo.CurrentValidator)
                .WithMessage(ValidationMessages.PluralMustBeEmpty)
                //
                .NotEmpty()
                .When(x => x.Type == WordTypeEnum.Noun, ApplyConditionTo.CurrentValidator)
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Plural)))
                //
                .MaximumLength(WordConstants.PluralMaxLength)
                .WithMessage(x => string.Format(ValidationMessages.MaximumLength, nameof(x.Plural), WordConstants.PluralMaxLength));

            RuleFor(x => x.Type)
                .NotEmpty()
                .NotEqual(WordTypeEnum.None)
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Type)));

            RuleFor(x => x.Conjugation)
                .Empty()
                .When(x => x.Type != WordTypeEnum.Verb, ApplyConditionTo.CurrentValidator)
                .WithMessage(ValidationMessages.ConjugationMustBeEmpty)
                //
                .NotEmpty()
                .When(x => x.Type == WordTypeEnum.Verb, ApplyConditionTo.CurrentValidator)
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Conjugation)))
                //
                .MaximumLength(WordConstants.ConjugationMaxLength)
                .WithMessage(x => string.Format(ValidationMessages.MaximumLength, nameof(x.Conjugation), WordConstants.ConjugationMaxLength));

            RuleFor(x => x.LanguageCode)
                .NotEmpty()
                .NotEqual(LanguageCodeEnum.None)
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.LanguageCode)));
        }
    }
}
