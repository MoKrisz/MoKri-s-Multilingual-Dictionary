using Dictionary.Models.Builders;
using Dictionary.Models.Dtos;
using Dictionary.Models.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MoKrisMultilingualDictionary.Controllers;
using System.Text.Json;

namespace Dictionary.Tests.IntegrationTests.WebApi.ControllerTests
{
    public class WordTests : IntegrationTestBase
    {
        public WordTests(WebApplicationFactoryWithDbContext<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetWordTest()
        {
            var dbContext = GetDatabase();

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
            var response = await _client.GetAsync(url);
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