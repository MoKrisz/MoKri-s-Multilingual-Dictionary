using Dictionary.Data;
using Dictionary.Domain;
using Dictionary.Domain.Builders;
using Dictionary.Domain.Enums;
using Dictionary.Models.Dtos;
using Dictionary.Tests.Common.Factories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoKrisMultilingualDictionary.Routes;
using System.Net.Http.Json;

namespace Dictionary.Tests.IntegrationTests.WebApi.ControllerTests
{
    [Collection("IntegrationTestCollection")]
    public class PracticeTests
    {
        private readonly IntegrationTestFixture<Program> fixture;
        private readonly HttpClient client;

        public PracticeTests(IntegrationTestFixture<Program> fixture) 
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            client = fixture.Client;
        }

        [Fact]
        public async Task GetGuessArticleRandomWordsTest_ReturnsOnlyNouns_ForRequestedLanguage()
        {
            var db = await fixture.GetDatabase();

            var expectedGermanNouns = await SeedGuessArticleWords(db);

            var url = $"/{PracticeRoutes.ControllerBaseRoute}/{PracticeRoutes.GetGuessArticleRandomWords}?amount={expectedGermanNouns.Count}&languageCode={(int)LanguageCodeEnum.DE}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<GuessArticleWordDto>>();

            result.Should().NotBeNull();
            result.Should().OnlyContain(x =>
                expectedGermanNouns.Any(pw => pw.WordId == x.WordId && pw.Text == x.Text));
        }

        [Fact]
        public async Task GetGuessArticleRandomWordsTest_ReturnsRequestedAmount_WhenEnoughMatchingWordsExist()
        {
            var db = await fixture.GetDatabase();

            var expectedGermanNouns = await SeedGuessArticleWords(db);

            var amount = expectedGermanNouns.Count - 2;

            var url = $"/{PracticeRoutes.ControllerBaseRoute}/{PracticeRoutes.GetGuessArticleRandomWords}?amount={amount}&languageCode={(int)LanguageCodeEnum.DE}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<GuessArticleWordDto>>();

            result.Should().NotBeNull();
            result!.Count().Should().Be(amount);
        }

        [Fact]
        public async Task GetGuessArticleRandomWordsTest_ReturnsAllMatchingWords_WhenRequestedAmountIsGreaterThanAvailable()
        {
            var db = await fixture.GetDatabase();

            var expectedGermanNouns = await SeedGuessArticleWords(db);

            var amount = expectedGermanNouns.Count + 2;

            var url = $"/{PracticeRoutes.ControllerBaseRoute}/{PracticeRoutes.GetGuessArticleRandomWords}?amount={amount}&languageCode={(int)LanguageCodeEnum.DE}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<GuessArticleWordDto>>();

            result.Should().NotBeNull();
            result!.Count().Should().Be(expectedGermanNouns.Count);
        }

        [Fact]
        public async Task GetGuessArticleRandomWordsTest_ReturnsEmptyList_WhenNoMatchingWordsExist()
        {
            var db = await fixture.GetDatabase();

            await SeedGuessArticleWords(db);

            var url = $"/{PracticeRoutes.ControllerBaseRoute}/{PracticeRoutes.GetGuessArticleRandomWords}?amount={1}&languageCode={(int)LanguageCodeEnum.HU}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<GuessArticleWordDto>>();

            result.Should().NotBeNull();
            result!.Should().BeEmpty();
        }

        [Fact]
        public async Task EvaluateGuessArticle_WhenRequestIsValid_ReturnsEvaluatedGuesses()
        {
            var db = await fixture.GetDatabase();

            var words = await SeedGuessArticleWords(db, 2);

            var url = $"/{PracticeRoutes.ControllerBaseRoute}/{PracticeRoutes.PostGuessArticleEvaluation}";
            var requestDto = new EvaluateGuessArticleRequestDto
            {
                Guesses = new List<EvaluateGuessArticleRequestItemDto>
                {
                    new() { WordId = words[0].WordId, Answer = words[0].Article! },
                    new() { WordId = words[1].WordId, Answer = words[1].Article!+"a" },
                }
            };

            var response = await client.PostAsJsonAsync(url, requestDto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<EvaluateGuessArticleResponseItemDto>>();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result.Should().ContainEquivalentOf(new EvaluateGuessArticleResponseItemDto
            {
                WordId = words[0].WordId,
                Text = words[0].Text,
                Answer = requestDto.Guesses[0].Answer,
                CorrectArticle = words[0].Article!,
                IsCorrect = true
            });

            result.Should().ContainEquivalentOf(new EvaluateGuessArticleResponseItemDto
            {
                WordId = words[1].WordId,
                Text = words[1].Text,
                Answer = requestDto.Guesses[1].Answer,
                CorrectArticle = words[1].Article!,
                IsCorrect = false
            });
        }

        [Fact]
        public async Task EvaluateGuessArticle_WhenSomeGuessesAreNotEvaluable_ReturnsOnlyEvaluableWords()
        {
            var db = await fixture.GetDatabase();

            var words = await SeedGuessArticleWords(db, 2);

            var verb = await db.Words.FirstAsync(w => w.Type == WordTypeEnum.Verb);

            var englishNoun = await db.Words.FirstAsync(w => w.LanguageCode == LanguageCodeEnum.EN && w.Type == WordTypeEnum.Noun);

            var url = $"/{PracticeRoutes.ControllerBaseRoute}/{PracticeRoutes.PostGuessArticleEvaluation}";
            var requestDto = new EvaluateGuessArticleRequestDto
            {
                Guesses = new List<EvaluateGuessArticleRequestItemDto>
                {
                    new() { WordId = words[0].WordId, Answer = words[0].Article! },
                    new() { WordId = verb.WordId, Answer = "das" },
                    new() { WordId = englishNoun.WordId, Answer = string.Empty }
                }
            };

            var response = await client.PostAsJsonAsync(url, requestDto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<EvaluateGuessArticleResponseItemDto>>();

            result.Should().NotBeNull();
            result.Should().ContainSingle();

            result.Should().ContainEquivalentOf(new EvaluateGuessArticleResponseItemDto
            {
                WordId = words[0].WordId,
                Text = words[0].Text,
                Answer = requestDto.Guesses[0].Answer,
                CorrectArticle = words[0].Article!,
                IsCorrect = true
            });
        }

        private async Task<List<Word>> SeedGuessArticleWords(DictionaryContext dbContext, int nounCount = 8)
        {
            List<Word> words = new List<Word>();
            for (int i = 0; i < nounCount; i++)
            {
                var noun = WordFactory.GermanNoun("das", "Test_" + i, "Tests_" + i);

                words.Add(noun);
            }

            var otherLanguageNoun = WordFactory.EnglishNoun();

            var verb = WordFactory.GermanVerb();

            List<Word> addableWord = [..words, otherLanguageNoun, verb];

            await dbContext.Words.AddRangeAsync(addableWord);

            await dbContext.SaveChangesAsync();

            return words;
        }
    }
}
