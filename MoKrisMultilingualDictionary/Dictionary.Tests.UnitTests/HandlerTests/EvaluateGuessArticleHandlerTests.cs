using Dictionary.BusinessLogic.Practice.GuessArticle.Handlers;
using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.Data;
using Dictionary.Models.Dtos;
using Dictionary.Tests.Common.Factories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Tests.UnitTests.HandlerTests
{
    public class EvaluateGuessArticleHandlerTests
    {
        [Fact]
        public async Task Handle_WhenGuessIsCorrect_ReturnsCorrectResult()
        {
            await using var dbContext = GetInMemoryDbContext();

            var noun = WordFactory.GermanNoun();
            dbContext.Words.Add(noun);

            await dbContext.SaveChangesAsync();

            var handler = new EvaluateGuessArticleHandler(dbContext);

            var request = new EvaluateGuessArticleRequest
            {
                Guesses = new List<EvaluateGuessArticleRequestItemDto>
                {
                    new()
                    {
                        WordId = noun.WordId,
                        Answer = noun.Article!
                    }
                }
            };

            var result = await handler.Handle(request, default);

            result.Should().ContainSingle();

            var item = result.Single();
            item.WordId.Should().Be(noun.WordId);
            item.Text.Should().Be(noun.Text);
            item.Answer.Should().Be(request.Guesses[0].Answer);
            item.CorrectArticle.Should().Be(noun.Article);
            item.IsCorrect.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WhenGuessIsIncorrect_ReturnsIncorrectResult()
        {
            await using var dbContext = GetInMemoryDbContext();

            var noun = WordFactory.GermanNoun();
            dbContext.Words.Add(noun);

            await dbContext.SaveChangesAsync();

            var handler = new EvaluateGuessArticleHandler(dbContext);

            var request = new EvaluateGuessArticleRequest
            {
                Guesses = new List<EvaluateGuessArticleRequestItemDto>
                {
                    new()
                    {
                        WordId = noun.WordId,
                        Answer = noun.Article+"a"
                    }
                }
            };

            var result = await handler.Handle(request, default);

            result.Should().ContainSingle();

            var item = result.Single();
            item.WordId.Should().Be(noun.WordId);
            item.Text.Should().Be(noun.Text);
            item.Answer.Should().Be(request.Guesses[0].Answer);
            item.CorrectArticle.Should().Be(noun.Article);
            item.IsCorrect.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_WhenWordDoesNotExist_IgnoresGuess()
        {
            await using var dbContext = GetInMemoryDbContext();

            var handler = new EvaluateGuessArticleHandler(dbContext);

            var request = new EvaluateGuessArticleRequest
            {
                Guesses = new List<EvaluateGuessArticleRequestItemDto>
                {
                    new()
                    {
                        WordId = 1,
                        Answer = "der"
                    }
                }
            };

            var result = await handler.Handle(request, default);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WhenWordIsNotNoun_IgnoresGuess()
        {
            await using var dbContext = GetInMemoryDbContext();

            var verb = WordFactory.GermanVerb();
            dbContext.Words.Add(verb);

            await dbContext.SaveChangesAsync();

            var handler = new EvaluateGuessArticleHandler(dbContext);

            var request = new EvaluateGuessArticleRequest
            {
                Guesses = new List<EvaluateGuessArticleRequestItemDto>
                {
                    new()
                    {
                        WordId = verb.WordId,
                        Answer = "der"
                    }
                }
            };

            var result = await handler.Handle(request, default);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WhenLanguageDoesNotSupportArticles_IgnoresGuess()
        {
            await using var dbContext = GetInMemoryDbContext();

            var noun = WordFactory.EnglishNoun();
            dbContext.Words.Add(noun);

            await dbContext.SaveChangesAsync();

            var handler = new EvaluateGuessArticleHandler(dbContext);

            var request = new EvaluateGuessArticleRequest
            {
                Guesses = new List<EvaluateGuessArticleRequestItemDto>
                {
                    new()
                    {
                        WordId = noun.WordId,
                        Answer = "the"
                    }
                }
            };

            var result = await handler.Handle(request, default);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WhenMultipleGuesses_ReturnsOnlyValidEvaluatedNouns()
        {
            await using var dbContext = GetInMemoryDbContext();

            var noun1 = WordFactory.GermanNoun();
            var noun2 = WordFactory.GermanNoun("der", "Tisch", "Tische");
            var noun3 = WordFactory.EnglishNoun();
            var verb = WordFactory.GermanVerb();

            dbContext.Words.AddRange(noun1, noun2, noun3, verb);

            await dbContext.SaveChangesAsync();

            var handler = new EvaluateGuessArticleHandler(dbContext);

            var request = new EvaluateGuessArticleRequest
            {
                Guesses = new List<EvaluateGuessArticleRequestItemDto>
                {
                    new()
                    {
                        WordId = noun1.WordId,
                        Answer = noun1.Article!
                    },
                    new()
                    {
                        WordId = noun2.WordId,
                        Answer = noun2.Article+"a"
                    },
                    new()
                    {
                        WordId = noun3.WordId,
                        Answer = "the"
                    },
                    new()
                    {
                        WordId = verb.WordId,
                        Answer = string.Empty
                    },
                    new()
                    {
                        WordId = 999,
                        Answer = "die"
                    }
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            result.Should().HaveCount(2);

            var assertNoun1 = result.First(r => r.WordId == noun1.WordId);
            assertNoun1.Text.Should().Be(noun1.Text);
            assertNoun1.Answer.Should().Be(request.Guesses[0].Answer);
            assertNoun1.CorrectArticle.Should().Be(noun1.Article);
            assertNoun1.IsCorrect.Should().BeTrue();

            var assertNoun2 = result.First(r => r.WordId == noun2.WordId);
            assertNoun2.Text.Should().Be(noun2.Text);
            assertNoun2.Answer.Should().Be(request.Guesses[1].Answer);
            assertNoun2.CorrectArticle.Should().Be(noun2.Article);
            assertNoun2.IsCorrect.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_WhenGuessesAreEmpty_ReturnsEmptyResult()
        {
            await using var dbContext = GetInMemoryDbContext();

            var handler = new EvaluateGuessArticleHandler(dbContext);

            var request = new EvaluateGuessArticleRequest
            {
                Guesses = new List<EvaluateGuessArticleRequestItemDto>()
            };

            var result = await handler.Handle(request, default);

            result.Should().BeEmpty();
        }

        private DictionaryContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DictionaryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new DictionaryContext(options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
