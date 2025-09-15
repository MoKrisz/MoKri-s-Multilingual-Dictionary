using Dictionary.BusinessLogic.Tag.Services;
using Dictionary.Data;
using Dictionary.Domain.Builders;
using Dictionary.Models.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Tests.UnitTests.ServiceTests
{
    public class TagServiceTests
    {
        [Fact]
        public async Task GetOrCreateTagsAsync_AllNewTags_Test()
        {
            await using var dbContext = GetInMemoryDbContext();

            var tagService = new TagService(dbContext);

            var tagDtos = new List<TagDto>()
            {
                new TagDto() { TagId = 1, Text = "testtag1"},
                new TagDto() { Text = " TESTTAG2 " }
            };

            var result = await tagService.GetOrCreateTagsAsync(tagDtos, default);

            result.Should().HaveCount(2);

            result.Any(r => r.TagId != default).Should().BeFalse();
            result.Should().Contain(r => r.Text.Equals(tagDtos[0].Text));
            result.Should().Contain(r => r.Text.Equals(tagDtos[1].Text.Trim().ToLower()));
        }

        [Fact]
        public async Task GetOrCreateTagsAsync_AllExistingTags_Test()
        {
            await using var dbContext = GetInMemoryDbContext();

            var existingTag = new TagBuilder().SetTagId(1).SetText("testtag1").Build();
            var existingTagByName = new TagBuilder().SetTagId(2).SetText("testtag2").Build();

            await dbContext.Tags.AddRangeAsync([existingTag, existingTagByName]);
            await dbContext.SaveChangesAsync();

            var tagService = new TagService(dbContext);

            var tagDtos = new List<TagDto>()
            {
                new TagDto() { TagId = existingTag.TagId, Text = existingTag.Text},
                new TagDto() { Text = existingTagByName.Text }
            };

            var result = await tagService.GetOrCreateTagsAsync(tagDtos, default);

            result.Should().HaveCount(2);

            result.Any(r => r.TagId == default).Should().BeFalse();
            result.Should().Contain(r => r.TagId == tagDtos[0].TagId 
                                        && r.Text.Equals(tagDtos[0].Text));
            result.Should().Contain(r => r.TagId == existingTagByName.TagId
                                        && r.Text.Equals(tagDtos[1].Text));
        }

        [Fact]
        public async Task GetOrCreateTagsAsync_MixedNewAndExistingTags_Test()
        {
            await using var dbContext = GetInMemoryDbContext();

            var existingTag = new TagBuilder().SetTagId(1).SetText("testtag1").Build();
            var existingTagByName = new TagBuilder().SetTagId(2).SetText("testtag2").Build();

            await dbContext.Tags.AddRangeAsync([existingTag, existingTagByName]);
            await dbContext.SaveChangesAsync();

            var tagService = new TagService(dbContext);

            var tagDtos = new List<TagDto>()
            {
                new TagDto() { TagId = existingTag.TagId, Text = existingTag.Text},
                new TagDto() { Text = existingTagByName.Text },
                new TagDto() { TagId = 3, Text = "testtag3" },
                new TagDto() { Text = "testtag4" }
            };

            var result = await tagService.GetOrCreateTagsAsync(tagDtos, default);

            result.Should().HaveCount(4);

            result.Count(r => r.TagId == default).Should().Be(2);
            result.Count(r => r.TagId != default).Should().Be(2);

            result.Should().Contain(r => r.TagId == tagDtos[0].TagId
                                        && r.Text.Equals(tagDtos[0].Text));
            result.Should().Contain(r => r.TagId == existingTagByName.TagId
                                        && r.Text.Equals(tagDtos[1].Text));
            result.Should().Contain(r => r.TagId == default
                                        && r.Text.Equals(tagDtos[2].Text));
            result.Should().Contain(r => r.TagId == default
                                        && r.Text.Equals(tagDtos[3].Text));
        }

        private DictionaryContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DictionaryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new DictionaryContext(options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
