using Dictionary.Domain.Builders;
using Dictionary.Domain.Enums;
using Dictionary.Models.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoKrisMultilingualDictionary.Routes;
using System.Net.Http.Json;
using System.Text.Json;

namespace Dictionary.Tests.IntegrationTests.WebApi.ControllerTests
{
    [Collection("IntegrationTestCollection")]
    public class TranslationGroupTests
    {
        private readonly IntegrationTestFixture<Program> fixture;
        private readonly HttpClient client;

        public TranslationGroupTests(IntegrationTestFixture<Program> fixture) 
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            client = fixture.Client;
        }

        [Fact]
        public async Task PostTranslationGroupTest()
        {
            var db = await fixture.GetDatabase();

            var tag = new TagBuilder().SetText("testtag1").Build();

            await db.Tags.AddAsync(tag);
            await db.SaveChangesAsync();

            var translationGroupDto = new TranslationGroupDto
            {
                Description = "description",
                Tags = new List<TagDto>
                {
                    new TagDto() { TagId = tag.TagId, Text = tag.Text },
                    new TagDto() { Text = "TestTag2" }
                }
            };

            var url = $"/{TranslationGroupRoutes.ControllerBaseRoute}";
            var response = await client.PostAsJsonAsync(url, translationGroupDto);
            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TranslationGroupDto>(jsonResult);

            var assertTranslationGroup = await db.TranslationGroups
                .AsNoTracking()
                .Include(tg => tg.TranslationGroupTags)
                    .ThenInclude(tgt => tgt.Tag)
                .SingleAsync();

            result.Should().NotBeNull();
            assertTranslationGroup.TranslationGroupId.Should().Be(result!.TranslationGroupId);
            assertTranslationGroup.Description.Should().Be(translationGroupDto.Description);
            assertTranslationGroup.TranslationGroupTags.Count.Should().Be(translationGroupDto.Tags.Count);
            result.Description.Should().Be(translationGroupDto.Description);
            result.Tags.Count.Should().Be(translationGroupDto.Tags.Count);

            (await db.TranslationGroupTags.CountAsync()).Should().Be(translationGroupDto.Tags.Count);
            (await db.Tags.CountAsync()).Should().Be(translationGroupDto.Tags.Count);

            foreach (var tagDto in translationGroupDto.Tags)
            {
                assertTranslationGroup.TranslationGroupTags.Any(tgt => tgt.Tag.Text == tagDto.Text.ToLowerInvariant()).Should().BeTrue();
                result.Tags.Any(t => t.Text == tagDto.Text.ToLowerInvariant()).Should().BeTrue();
            }

        }

        [Fact]
        public async Task GetWordRelatedTranslationGroupsTest()
        {
            var db = await fixture.GetDatabase();

            var linkedTranslationGroup = new TranslationGroupBuilder()
                .SetDescription("Test1")
                .Build();

            var potentialTranslationGroup1 = new TranslationGroupBuilder()
                .SetDescription("Test2")
                .Build();

            var potentialTranslationGroup2 = new TranslationGroupBuilder()
                .SetDescription("Test3")
                .Build();

            var sourceWord = new WordBuilder()
                .SetText("Test")
                .SetPlural("Tests")
                .SetType(WordTypeEnum.Noun)
                .SetLanguageCode(LanguageCodeEnum.EN)
                .Build();

            var targetWord = new WordBuilder()
                .SetArticle("Die")
                .SetText("Test")
                .SetPlural("Tests")
                .SetType(WordTypeEnum.Noun)
                .SetLanguageCode(LanguageCodeEnum.DE)
                .Build();

            var wordTranslationGroups = new []
            {
                new WordTranslationGroupBuilder()
                    .SetWord(sourceWord)
                    .SetTranslationGroup(linkedTranslationGroup)
                    .Build(),
                new WordTranslationGroupBuilder()
                    .SetWord(targetWord)
                    .SetTranslationGroup(linkedTranslationGroup)
                    .Build(),
                 new WordTranslationGroupBuilder()
                    .SetWord(sourceWord)
                    .SetTranslationGroup(potentialTranslationGroup1)
                    .Build(),
                new WordTranslationGroupBuilder()
                    .SetWord(targetWord)
                    .SetTranslationGroup(potentialTranslationGroup2)
                    .Build(),
            };


            await db.Words.AddRangeAsync(sourceWord, targetWord);
            await db.TranslationGroups.AddRangeAsync(linkedTranslationGroup, potentialTranslationGroup1, potentialTranslationGroup2);
            await db.WordTranslationGroups.AddRangeAsync(wordTranslationGroups);
            await db.SaveChangesAsync();

            var url = $"/{TranslationGroupRoutes.ControllerBaseRoute}/{TranslationGroupRoutes.WordRelatedTranslationGroupsRoute}?sourceWordId={sourceWord.WordId}&targetWordId={targetWord.WordId}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<WordRelatedTranslationGroupsDto>(jsonResult);

            result.Should().NotBeNull();
            result!.LinkedTranslationGroups.Count.Should().Be(1);
            result.LinkedTranslationGroups[0].TranslationGroupId.Should().Be(linkedTranslationGroup.TranslationGroupId);
            result.PotentialTranslationGroups.Count.Should().Be(2);
            result.PotentialTranslationGroups.Any(ptg => ptg.TranslationGroupId == potentialTranslationGroup1.TranslationGroupId).Should().BeTrue();
            result.PotentialTranslationGroups.Any(ptg => ptg.TranslationGroupId == potentialTranslationGroup2.TranslationGroupId).Should().BeTrue();
        }
    }
}
