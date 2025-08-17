using Dictionary.Domain.Builders;
using Dictionary.Domain.Enums;
using Dictionary.Models.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoKrisMultilingualDictionary.Routes;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Dictionary.Tests.IntegrationTests.WebApi.ControllerTests
{
    [Collection("IntegrationTestCollection")]
    public class TranslationTests
    {
        private readonly IntegrationTestFixture<Program> fixture;
        private readonly HttpClient client;

        public TranslationTests(IntegrationTestFixture<Program> fixture) 
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            client = fixture.Client;
        }

        [Fact]
        public async Task PostTranslationTest()
        {
            var db = await fixture.GetDatabase();

            var sourceWord = new WordBuilder()
                .SetText("Test")
                .SetPlural("Tests")
                .SetType(WordTypeEnum.Noun)
                .SetLanguageCode(LanguageCodeEnum.EN)
                .Build();

            var targetWord = new WordBuilder()
                .SetArticle("Das")
                .SetText("Test")
                .SetPlural("Tests")
                .SetType(WordTypeEnum.Noun)
                .SetLanguageCode(LanguageCodeEnum.DE)
                .Build();

            var commonTranslationGroup = new TranslationGroupBuilder()
                .SetDescription("Test1")
                .Build();

            var partiallyNewTranslationGroup = new TranslationGroupBuilder()
                .SetDescription("Test2")
                .Build();

            var totallyNewTranslationGroup = new TranslationGroupBuilder()
                .SetDescription("Test3")
                .Build();

            var wordTranslationGroup1 = new WordTranslationGroupBuilder()
                .SetWord(sourceWord)
                .SetTranslationGroup(commonTranslationGroup)
                .Build();

            var wordTranslationGroup2 = new WordTranslationGroupBuilder()
                .SetWord(targetWord)
                .SetTranslationGroup(commonTranslationGroup)
                .Build();

            var wordTranslationGroup3 = new WordTranslationGroupBuilder()
                .SetWord(sourceWord)
                .SetTranslationGroup(partiallyNewTranslationGroup)
                .Build();

            await db.Words.AddRangeAsync(sourceWord, targetWord);
            await db.TranslationGroups.AddRangeAsync(commonTranslationGroup, partiallyNewTranslationGroup, totallyNewTranslationGroup);
            await db.WordTranslationGroups.AddRangeAsync(wordTranslationGroup1, wordTranslationGroup2, wordTranslationGroup3);
            await db.SaveChangesAsync();

            var translationDto = new TranslationDto
            {
                SourceWordId = sourceWord.WordId,
                TargetWordId = targetWord.WordId,
                LinkedTranslationGroups = new List<TranslationGroupDto>
                {
                    new TranslationGroupDto() { TranslationGroupId = commonTranslationGroup.TranslationGroupId, Description = commonTranslationGroup.Description },
                    new TranslationGroupDto() { TranslationGroupId = partiallyNewTranslationGroup.TranslationGroupId, Description = partiallyNewTranslationGroup.Description  },
                    new TranslationGroupDto() { TranslationGroupId = totallyNewTranslationGroup.TranslationGroupId, Description = totallyNewTranslationGroup.Description  }
                }
            };

            var url = $"/{TranslationRoutes.ControllerBaseRoute}";
            var response = await client.PostAsJsonAsync(url, translationDto);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            (await db.WordTranslationGroups.CountAsync()).Should().Be(6);

            var existingWordTranslationGroupIds = new[] { wordTranslationGroup1.WordTranslationGroupId, wordTranslationGroup2.WordTranslationGroupId, wordTranslationGroup3.WordTranslationGroupId };
            var assertWordTranslationGroups = await db.WordTranslationGroups
                .Where(wtg => !existingWordTranslationGroupIds.Contains(wtg.WordTranslationGroupId))
                .ToListAsync();

            assertWordTranslationGroups.Count.Should().Be(3);
            assertWordTranslationGroups.Single(wtg => wtg.TranslationGroupId == partiallyNewTranslationGroup.TranslationGroupId)
                .WordId.Should().Be(targetWord.WordId);
            assertWordTranslationGroups.SingleOrDefault(wtg => 
                wtg.TranslationGroupId == totallyNewTranslationGroup.TranslationGroupId
                && wtg.WordId == sourceWord.WordId).Should().NotBeNull();
            assertWordTranslationGroups.SingleOrDefault(wtg =>
                wtg.TranslationGroupId == totallyNewTranslationGroup.TranslationGroupId
                && wtg.WordId == targetWord.WordId).Should().NotBeNull();
        }
    }
}
