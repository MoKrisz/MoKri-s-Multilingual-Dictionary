using Dictionary.Domain.Builders;
using Dictionary.Models.Dtos;
using Dictionary.Tests.IntegrationTests.WebApi.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dictionary.Tests.IntegrationTests.WebApi.ControllerTests
{
    [Collection("IntegrationTestCollection")]
    public class TranslationGroupListTests
    {
        private readonly IntegrationTestFixture<Program> fixture;
        private readonly HttpClient client;

        public TranslationGroupListTests(IntegrationTestFixture<Program> fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            client = fixture.Client;
        }

        [Fact]
        public async Task GetTranslationGroupList_WithoutFilteringTest()
        {
            var dbContext = await fixture.GetDatabase();
            await SetupData(dbContext);

            var url = $"/odata/TranslationGroupList?$count=true";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ODataResponse<TranslationGroupDto>>(jsonResult);

            var translationGroupCount = await dbContext.TranslationGroups.CountAsync();
            result!.Count.Should().Be(translationGroupCount);
            result.Values.All(v => !string.IsNullOrEmpty(v.Description)).Should().BeTrue();
            result.Values.All(v => v.Tags.Count > 0).Should().BeTrue();
        }

        [Fact]
        public async Task GetTranslationGroupList_WithFilteringTest()
        {
            var dbContext = await fixture.GetDatabase();
            await SetupData(dbContext);

            var url = $"/odata/TranslationGroupList?$count=true&filter=tags/any(t: t/text eq 'Tag2')";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ODataResponse<TranslationGroupDto>>(jsonResult);

            result!.Count.Should().Be(1);
            result.Values.First().Description.Should().Be("Test");
        }

        private async Task SetupData(Data.DictionaryContext dbContext)
        {
            var tag = new TagBuilder()
                .SetText("Tag1")
                .Build();

            var tag2 = new TagBuilder()
                .SetText("Tag2")
                .Build();

            var translationGroup = new TranslationGroupBuilder()
                .SetDescription("Test")
                .Build();

            var translationGroup2 = new TranslationGroupBuilder()
                .SetDescription("Test2")
                .Build();

            var translationGroupTag = new TranslationGroupTagBuilder()
                .SetTranslationGroup(translationGroup)
                .SetTag(tag)
                .Build();

            var translationGroupTag2 = new TranslationGroupTagBuilder()
                .SetTranslationGroup(translationGroup)
                .SetTag(tag2)
                .Build();

            var translationGroupTag3 = new TranslationGroupTagBuilder()
                .SetTranslationGroup(translationGroup2)
                .SetTag(tag)
                .Build();

            await dbContext.Tags.AddRangeAsync(new[] { tag, tag2 });
            await dbContext.TranslationGroups.AddRangeAsync(new[] { translationGroup, translationGroup2 });
            await dbContext.TranslationGroupTags.AddRangeAsync(new[] { translationGroupTag, translationGroupTag2, translationGroupTag3 });

            await dbContext.SaveChangesAsync();
        }
    }
}