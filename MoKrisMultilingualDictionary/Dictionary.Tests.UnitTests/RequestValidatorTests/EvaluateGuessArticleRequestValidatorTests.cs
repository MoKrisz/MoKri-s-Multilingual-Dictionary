using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.BusinessLogic.Practice.GuessArticle.Validators;
using Dictionary.Models.Dtos;
using FluentValidation.TestHelper;

namespace Dictionary.Tests.UnitTests.RequestValidatorTests
{
    public class EvaluateGuessArticleRequestValidatorTests
    {
        private readonly EvaluateGuessArticleRequestValidator validator = new();

        [Theory]
        [InlineData(0)]
        [InlineData(EvaluateGuessArticleRequestValidator.MaxWordAmount)]
        [InlineData(EvaluateGuessArticleRequestValidator.MaxWordAmount - 1)]
        public void GuessCount_WhenLesserOrEqualToMax_ShouldNotHaveError(int guessCount)
        {
            var request = CreateRequest(guessCount);

            var result = validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Guesses);
        }

        [Fact]
        public void GuessCount_WhenGreaterThanMax_ShouldHaveError()
        {
            var request = CreateRequest(EvaluateGuessArticleRequestValidator.MaxWordAmount + 1);

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Guesses);
        }

        private static EvaluateGuessArticleRequest CreateRequest(int guessCount = 1)
        {
            List<EvaluateGuessArticleRequestItemDto> guesses = new List<EvaluateGuessArticleRequestItemDto>();

            for (int i = 0; i < guessCount; i++)
            {
                guesses.Add(new EvaluateGuessArticleRequestItemDto
                {
                    WordId = i
                });
            }

            return new EvaluateGuessArticleRequest
            {
                Guesses = guesses
            };
        }
    }
}
