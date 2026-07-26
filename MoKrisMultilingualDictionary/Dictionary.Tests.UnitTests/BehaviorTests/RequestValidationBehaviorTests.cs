using Dictionary.BusinessLogic.Behaviors;
using Dictionary.BusinessLogic.Exceptions;
using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.BusinessLogic.Practice.GuessArticle.Validators;
using Dictionary.Domain.Enums;
using FluentAssertions;
using FluentValidation;
using MediatR;

namespace Dictionary.Tests.UnitTests.BehaviorTests
{
    public class RequestValidationBehaviorTests
    {
        [Fact]
        public async Task Handle_WhenValidationPasses_ShouldCallNext()
        {
            var validators = new List<IValidator<GetGuessArticleRandomWordsRequest>>()
            {
                new GetGuessArticleRandomWordsRequestValidator()
            };

            var behavior = new RequestValidationBehavior<GetGuessArticleRandomWordsRequest, bool>(validators);

            var request = new GetGuessArticleRandomWordsRequest
            {
                Amount = GetGuessArticleRandomWordsRequestValidator.MinWordAmount,
                LanguageCode = (int)LanguageCodeEnum.EN
            };

            var nextWasCalled = false;

            RequestHandlerDelegate<bool> next = () =>
            {
                nextWasCalled = true;
                return Task.FromResult(true);
            };

            var result = await behavior.Handle(request, next, default);

            nextWasCalled.Should().BeTrue();
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WhenNoValidatorsExist_ShouldCallNext()
        {
            var behavior = new RequestValidationBehavior<GetGuessArticleRandomWordsRequest, bool>([]);

            var request = new GetGuessArticleRandomWordsRequest
            {
                Amount = GetGuessArticleRandomWordsRequestValidator.MinWordAmount,
                LanguageCode = (int)LanguageCodeEnum.EN
            };

            var nextWasCalled = false;

            RequestHandlerDelegate<bool> next = () =>
            {
                nextWasCalled = true;
                return Task.FromResult(true);
            };

            var result = await behavior.Handle(request, next, default);

            nextWasCalled.Should().BeTrue();
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WhenValidationFails_ShouldThrowRequestValidationExceptionAndNotCallNext()
        {
            var validators = new List<IValidator<GetGuessArticleRandomWordsRequest>>()
            {
                new GetGuessArticleRandomWordsRequestValidator()
            };

            var behavior = new RequestValidationBehavior<GetGuessArticleRandomWordsRequest, bool>(validators);

            var request = new GetGuessArticleRandomWordsRequest
            {
                Amount = GetGuessArticleRandomWordsRequestValidator.MinWordAmount - 1,
                LanguageCode = (int)LanguageCodeEnum.EN
            };

            var nextWasCalled = false;

            RequestHandlerDelegate<bool> next = () =>
            {
                nextWasCalled = true;
                return Task.FromResult(true);
            };

            var act = () => behavior.Handle(request, next, default);

            await act.Should().ThrowAsync<RequestValidationException>();

            nextWasCalled.Should().BeFalse();
        }
    }
}
