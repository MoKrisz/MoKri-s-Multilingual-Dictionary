using Dictionary.BusinessLogic.Services.Synchronization;
using Dictionary.BusinessLogic.Tag;
using Dictionary.Domain.Builders;
using Dictionary.Models.Dtos;
using FluentAssertions;

namespace Dictionary.Tests.UnitTests.ServiceTests
{
    public class DataSynchronizerTests
    {
        [Fact]
        public void TagComparerSynchronizeTest()
        {
            var synchronizer = new DataSynchronizer();
            var comparer = new TagComparer();

            var sourceTags = new List<Domain.Tag>()
            {
                new TagBuilder().SetTagId(1).SetText("testtag1").Build(),
                new TagBuilder().SetTagId(2).SetText("testtag2").Build(),
                new TagBuilder().SetTagId(3).SetText("testtag3").Build(),
            };

            var newTag1 = new TagDto() { TagId = 4, Text = "testtag4" };
            var newTag2 = new TagDto() { TagId = null, Text = "testtag5" };
            var modifiedTag = new TagDto() { TagId = sourceTags[1].TagId, Text = "modified" };
            var targetTags = new List<TagDto>()
            {
                new TagDto() { TagId = sourceTags[0].TagId, Text = sourceTags[0].Text },
                modifiedTag,
                newTag1,
                newTag2,
            };

            var result = synchronizer.Synchronize(sourceTags, targetTags, comparer);

            result.Added.Count.Should().Be(2);
            result.Added.SingleOrDefault(t => t.TagId == newTag1.TagId && t.Text.Equals(newTag1.Text)).Should().NotBeNull();
            result.Added.SingleOrDefault(t => t.TagId == newTag2.TagId && t.Text.Equals(newTag2.Text)).Should().NotBeNull();
            
            result.Deleted.Count.Should().Be(1);
            result.Deleted.First().TagId.Should().Be(sourceTags[2].TagId);
            
            result.Modified.Count.Should().Be(1);
            var modifiedResult = result.Modified.First();
            modifiedResult.Source.TagId.Should().Be(sourceTags[1].TagId);
            modifiedResult.Target.TagId.Should().Be(sourceTags[1].TagId);
            modifiedResult.Source.Text.Should().Be(sourceTags[1].Text);
            modifiedResult.Target.Text.Should().Be(modifiedTag.Text);
        }
    }
}
