using Dictionary.Domain.Builders;
using Dictionary.Models.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoKrisMultilingualDictionary.Routes;
using System.Net.Http.Json;

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

            var tag = new TagBuilder().SetText("TestTag1").Build();

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
            var resultId = Convert.ToInt32(jsonResult);

            var assertTranslationGroup = await db.TranslationGroups.AsNoTracking()
                .Include(tg => tg.TranslationGroupTags)
                    .ThenInclude(tgt => tgt.Tag)
                .SingleAsync();
            assertTranslationGroup.TranslationGroupId.Should().Be(resultId);
            assertTranslationGroup.Description.Should().Be(translationGroupDto.Description);
            assertTranslationGroup.TranslationGroupTags.Count.Should().Be(translationGroupDto.Tags.Count);

            (await db.TranslationGroupTags.CountAsync()).Should().Be(translationGroupDto.Tags.Count);

            foreach (var tagDto in translationGroupDto.Tags)
            {
                assertTranslationGroup.TranslationGroupTags.Any(tgt => tgt.Tag.Text == tagDto.Text).Should().BeTrue();
            }

            (await db.Tags.CountAsync()).Should().Be(translationGroupDto.Tags.Count);
        }
    }
}
