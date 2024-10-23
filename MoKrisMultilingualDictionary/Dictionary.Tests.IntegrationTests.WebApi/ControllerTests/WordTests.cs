using Dictionary.Models.Dtos;
using Dictionary.Models.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoKrisMultilingualDictionary.Controllers;
using System.Net.Http.Json;
using System.Text.Json;
using static Dictionary.Models.Word;

namespace Dictionary.Tests.IntegrationTests.WebApi.ControllerTests
{
    [Collection("IntegrationTestCollection")]
    public class WordTests
    {
        private readonly IntegrationTestFixture<Program> fixture;
        private readonly HttpClient client;

        public WordTests(IntegrationTestFixture<Program> fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            client = fixture.Client;
        }

        [Fact]
        public async Task GetWordTest()
        {
            var dbContext = await fixture.GetDatabase();

            var word = new WordBuilder()
                .SetArticle("die")
                .SetText("Mutter")
                .SetPlural("Mütter")
                .SetType(WordTypeEnum.Noun)
                .SetLanguageCode(LanguageCodeEnum.DE)
                .Build();
            await dbContext.Words.AddAsync(word);

            await dbContext.SaveChangesAsync();

            var url = $"/{WordController.GetWordRoute}?wordId={word.WordId}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<WordDto>(jsonResult);

            result.WordId.Should().BeGreaterThan(0);
            result.Article.Should().Be(word.Article);
            result.Text.Should().Be(word.Text);
            result.Plural.Should().Be(word.Plural);
            result.Type.Should().Be((int)word.Type);
            result.LanguageCode.Should().Be((int)word.LanguageCode);
        }

        [Fact]
        public async Task PostWordTest()
        {
            var dbContext = await fixture.GetDatabase();

            var wordDto = new WordDto
            {
                Article = "die",
                Text = "Mutter",
                Plural = "Mütter",
                Type = (int)WordTypeEnum.Noun,
                LanguageCode = (int)LanguageCodeEnum.DE
            };

            var url = $"/{WordController.PostWordRoute}";
            var response = await client.PostAsJsonAsync(url, wordDto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var resultId = Convert.ToInt32(result);

            dbContext.Words.Count().Should().Be(1);
            var assertWord = await dbContext.Words.FirstAsync();
            assertWord.WordId.Should().Be(resultId);
            assertWord.Article.Should().Be(wordDto.Article);
            assertWord.Text.Should().Be(wordDto.Text);
            assertWord.Plural.Should().Be(wordDto.Plural);
            assertWord.Type.Should().Be(WordTypeEnum.Noun);
            assertWord.LanguageCode.Should().Be(LanguageCodeEnum.DE);
        }
    }
}