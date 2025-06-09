using Dictionary.Domain.Builders;
using Dictionary.Domain.Enums;
using Dictionary.Models.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoKrisMultilingualDictionary.Controllers;
using System.Net.Http.Json;
using System.Text.Json;

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

            result.Should().NotBeNull();
            result!.WordId.Should().BeGreaterThan(0);
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

        [Fact]
        public async Task PutWordTest()
        {
            var dbContext = await fixture.GetDatabase();

            var word = new WordBuilder()
                .SetText("Test")
                .SetPlural("Tests")
                .SetType(WordTypeEnum.Noun)
                .SetLanguageCode(LanguageCodeEnum.EN)
                .Build();

            await dbContext.Words.AddAsync(word);

            await dbContext.SaveChangesAsync();

            var updateWordDto = new WordDto
            {
                WordId = word.WordId,
                Text = "test",
                Conjugation = "test/test/tests/test/test/test",
                Type = (int)WordTypeEnum.Verb,
                LanguageCode = (int)LanguageCodeEnum.EN
            };

            var url = $"/{WordController.PutWordRoute}";
            var response = await client.PutAsJsonAsync(url, updateWordDto);
            response.EnsureSuccessStatusCode();

            var assertDb = await fixture.GetDatabase(false);
            var assertWord = await assertDb.Words.SingleAsync(w => w.WordId == word.WordId);
            assertWord.Article.Should().Be(updateWordDto.Article);
            assertWord.Text.Should().Be(updateWordDto.Text);
            assertWord.Plural.Should().Be(updateWordDto.Plural);
            assertWord.Conjugation.Should().Be(updateWordDto.Conjugation);
            assertWord.Type.Should().Be(WordTypeEnum.Verb);
            assertWord.LanguageCode.Should().Be(LanguageCodeEnum.EN);
        }
    }
}