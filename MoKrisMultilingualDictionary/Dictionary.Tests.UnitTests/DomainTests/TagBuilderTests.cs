using Dictionary.Domain.Builders;
using Dictionary.Domain.Constants;
using FluentValidation;

namespace Dictionary.Tests.UnitTests.DomainTests
{
    public class TagBuilderTests
    {
        [Fact]
        public void ValidTagTest()
        {
            BuildTag(null, false);
        }

        public static TheoryData<string?, bool> TextValidationTestData => new()
        {
            { null, true },
            { string.Empty, true },
            { "Test", false },
            { new string('a', TagConstants.TextMaxLength), false },
            { new string('a', TagConstants.TextMaxLength + 1), true },
        };

        [Theory]
        [MemberData(nameof(TextValidationTestData))]
        public void TextValidationTest(string? text, bool shouldFail)
            => BuildTag(builder => builder.SetText(text), shouldFail);

        private static void BuildTag(Action<TagBuilder>? builderAction, bool shouldFail)
        {
            var builder = GetTagBuilderWithValidData();

            builderAction?.Invoke(builder);

            if (shouldFail)
            {
                Assert.Throws<ValidationException>(builder.Build);
            }
            else 
            {
                builder.Build();
            }
        }

        public static TagBuilder GetTagBuilderWithValidData()
        {
            return new TagBuilder().SetText("Test");
        }
    }
}