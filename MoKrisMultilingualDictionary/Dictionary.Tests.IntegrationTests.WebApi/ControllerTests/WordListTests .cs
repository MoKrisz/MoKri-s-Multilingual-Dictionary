using Dictionary.Domain.Builders;
using Dictionary.Domain.Enums;
using Dictionary.Models.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dictionary.Tests.IntegrationTests.WebApi.ControllerTests
{
    [Collection("IntegrationTestCollection")]
    public class WordListTests
    {
        private readonly IntegrationTestFixture<Program> fixture;
        private readonly HttpClient client;

        public WordListTests(IntegrationTestFixture<Program> fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            client = fixture.Client;
        }

        [Fact]
        public async Task GetWordList_WithoutFilteringTest()
        {
            var dbContext = await fixture.GetDatabase();
            await SetupData(dbContext);

            var url = $"/odata/WordList?$count=true";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ODataResponse<WordDto>>(jsonResult);

            var wordCount = await dbContext.Words.CountAsync();
            var nounCount = await dbContext.Words.CountAsync(w => w.Type == WordTypeEnum.Noun);
            var verbCount = await dbContext.Words.CountAsync(w => w.Type == WordTypeEnum.Verb);
            var adjectiveCount = await dbContext.Words.CountAsync(w => w.Type == WordTypeEnum.Adjective);
            result!.Count.Should().Be(wordCount);
            result.Values.All(v => !string.IsNullOrEmpty(v.Text)).Should().BeTrue();
            result.Values.Count(v => v.Type == (int)WordTypeEnum.Noun).Should().Be(nounCount);
            result.Values.Count(v => v.Type == (int)WordTypeEnum.Verb).Should().Be(verbCount);
            result.Values.Count(v => v.Type == (int)WordTypeEnum.Adjective).Should().Be(adjectiveCount);
        }

        [Fact]
        public async Task GetWordList_WithFilteringTest()
        {
            var dbContext = await fixture.GetDatabase();
            await SetupData(dbContext);

            var url = $"/odata/WordList?$count=true&filter=type eq 1";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ODataResponse<WordDto>>(jsonResult);

            var nounCount = await dbContext.Words.CountAsync(w => w.Type == WordTypeEnum.Noun);
            result!.Count.Should().Be(nounCount);
            result.Values.Count(v => v.Type == (int)WordTypeEnum.Noun).Should().Be(nounCount);
        }

        private async Task SetupData(Data.DictionaryContext dbContext)
        {
            var word = new WordBuilder()
                .SetArticle("die")
                .SetText("Mutter")
                .SetPlural("Mütter")
                .SetType(WordTypeEnum.Noun)
                .SetLanguageCode(LanguageCodeEnum.DE)
                .Build();

            var word2 = new WordBuilder()
                .SetText("swim")
                .SetType(WordTypeEnum.Verb)
                .SetConjugation("swim/swim/swims/swim/swim/swim")
                .SetLanguageCode(LanguageCodeEnum.EN)
                .Build();

            var word3 = new WordBuilder()
                .SetText("okos")
                .SetType(WordTypeEnum.Adjective)
                .SetLanguageCode(LanguageCodeEnum.HU)
                .Build();

            await dbContext.Words.AddRangeAsync(new[] { word, word2, word3 });

            await dbContext.SaveChangesAsync();
        }
    }

    public class ODataResponse<T>
    {
        [JsonPropertyName("@odata.count")]
        public int Count { get; set; }

        [JsonPropertyName("value")]
        public List<T> Values { get; set; } = default!;
    }
}