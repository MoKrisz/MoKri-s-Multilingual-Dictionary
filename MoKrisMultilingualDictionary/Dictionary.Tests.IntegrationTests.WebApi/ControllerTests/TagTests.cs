using Dictionary.Domain;
using Dictionary.Domain.Builders;
using Dictionary.Models.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoKrisMultilingualDictionary.Controllers;
using MoKrisMultilingualDictionary.Routes;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Dictionary.Tests.IntegrationTests.WebApi.ControllerTests
{
    [Collection("IntegrationTestCollection")]
    public class TagTests
    {
        private readonly IntegrationTestFixture<Program> fixture;
        private readonly HttpClient client;

        public TagTests(IntegrationTestFixture<Program> fixture) 
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            client = fixture.Client;
        }

        [Fact]
        public async Task GetClosestMatchingTagTest()
        {
            var db = await fixture.GetDatabase();

            var tag = new TagBuilder()
                .SetText("test")
                .Build();

            var tag2 = new TagBuilder()
                .SetText("tagtest")
                .Build();
            await db.Tags.AddRangeAsync(new []{ tag, tag2 });

            await db.SaveChangesAsync();

            var url = $"/{TagRoutes.ControllerBaseRoute}/{TagRoutes.GetClosestMatchingTagRoute}?tagText=Tag";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TagDto>(jsonResult);

            result!.TagId.Should().Be(tag2.TagId);
            result.Text.Should().Be(tag2.Text);
        }

        [Fact]
        public async Task GetClosestMatchingTag_NullTest()
        {
            var db = await fixture.GetDatabase();

            var tag = new TagBuilder()
                .SetText("test")
                .Build();

            await db.Tags.AddAsync(tag);

            await db.SaveChangesAsync();

            var url = $"/{TagRoutes.ControllerBaseRoute}/{TagRoutes.GetClosestMatchingTagRoute}?tagText=Tag";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var jsonResult = await response.Content.ReadAsStringAsync();
            jsonResult.Should().BeNullOrEmpty();
        }
    }
}
