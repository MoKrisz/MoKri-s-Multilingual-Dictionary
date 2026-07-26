using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.BusinessLogic.Practice.GuessArticle.Validators;
using Dictionary.Domain.Enums;
using FluentValidation.TestHelper;

namespace Dictionary.Tests.UnitTests.RequestValidatorTests
{
    public class GetGuessArticleRandomWordsRequestValidatorTests
    {
        private readonly GetGuessArticleRandomWordsRequestValidator validator = new();

        [Theory]
        [InlineData(GetGuessArticleRandomWordsRequestValidator.MinWordAmount)]
        [InlineData(GetGuessArticleRandomWordsRequestValidator.MinWordAmount + 1)]
        [InlineData(GetGuessArticleRandomWordsRequestValidator.MaxWordAmount)]
        public void Amount_WhenBetweenMinAndMax_ShouldNotHaveError(int amount)
        {
            var request = CreateRequest(amount);

            var result = validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Amount);
        }

        [Theory]
        [InlineData(GetGuessArticleRandomWordsRequestValidator.MinWordAmount - 1)]
        [InlineData(GetGuessArticleRandomWordsRequestValidator.MaxWordAmount + 1)]
        public void Amount_WhenOutsideMinAndMax_ShouldHaveError(int amount)
        {
            var request = CreateRequest(amount);

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }

        [Fact]
        public void LanguageCode_WhenValidEnumValue_ShouldNotHaveError()
        {
            var request = CreateRequest(languageCodeEnumIntValue: (int)LanguageCodeEnum.DE);

            var result = validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.LanguageCode);
        }

        [Fact]
        public void LanguageCode_WhenInvalidEnumValue_ShouldHaveError()
        {
            var request = CreateRequest(languageCodeEnumIntValue: -1);

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.LanguageCode);
        }

        private static GetGuessArticleRandomWordsRequest CreateRequest(int amount = 10, int languageCodeEnumIntValue = (int)LanguageCodeEnum.EN)
        {
            return new GetGuessArticleRandomWordsRequest
            {
                Amount = amount,
                LanguageCode = languageCodeEnumIntValue
            };
        }
    }
}
