using Dictionary.Data;
using Dictionary.Domain;
using Dictionary.Domain.Builders;
using Dictionary.Domain.Enums;
using Dictionary.Models.Dtos;
using FluentAssertions;
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

        private async Task<List<Word>> SeedGuessArticleWords(DictionaryContext dbContext)
        {
            const int nounCount = 8;

            List<Word> words = new List<Word>();
            for (int i = 0; i < nounCount; i++)
            {
                var noun = new WordBuilder()
                    .SetArticle("das")
                    .SetText("Test_"+i)
                    .SetPlural("Tests_"+i)
                    .SetType(WordTypeEnum.Noun)
                    .SetLanguageCode(LanguageCodeEnum.DE)
                    .Build();

                words.Add(noun);
            }

            var otherLanguageNoun = new WordBuilder()
                    .SetText("Test_Other")
                    .SetPlural("Tests_Other")
                    .SetType(WordTypeEnum.Noun)
                    .SetLanguageCode(LanguageCodeEnum.EN)
                    .Build();

            var verb = new WordBuilder()
                .SetText("tests")
                .SetConjugation("tests")
                .SetType(WordTypeEnum.Verb)
                .SetLanguageCode(LanguageCodeEnum.DE)
                .Build();

            List<Word> addableWord = [..words, otherLanguageNoun, verb];

            await dbContext.Words.AddRangeAsync(addableWord);

            await dbContext.SaveChangesAsync();

            return words;
        }
    }
}
