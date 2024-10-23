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
    }
}