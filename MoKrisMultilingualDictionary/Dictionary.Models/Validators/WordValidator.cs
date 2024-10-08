using Dictionary.Models;
using Dictionary.Models.Enums;
using Dictionary.Resources.Messages;
using FluentValidation;

namespace Dictionary.Validations
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
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Article)));

            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Text)));

            RuleFor(x => x.Plural)
                .Empty()
                .When(x => x.Type != WordTypeEnum.Noun, ApplyConditionTo.CurrentValidator)
                .WithMessage(ValidationMessages.PluralMustBeEmpty)
                //
                .NotEmpty()
                .When(x => x.Type == WordTypeEnum.Noun, ApplyConditionTo.CurrentValidator)
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Plural)));

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
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.Conjugation)));

            RuleFor(x => x.LanguageCode)
                .NotEmpty()
                .NotEqual(LanguageCodeEnum.None)
                .WithMessage(x => string.Format(ValidationMessages.MustHaveValue, nameof(x.LanguageCode)));
        }
    }
}
