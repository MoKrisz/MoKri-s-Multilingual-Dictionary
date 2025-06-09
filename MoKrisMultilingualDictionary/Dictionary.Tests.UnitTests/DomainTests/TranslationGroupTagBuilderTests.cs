using Dictionary.Domain;
using Dictionary.Domain.Builders;
using FluentValidation;

namespace Dictionary.Tests.UnitTests.DomainTests
{
    public class TranslationGroupTagBuilderTests
    {
        [Fact]
        public void ValidTranslationGroupTagTest()
        {
            BuildTranslationGroupTag(null, false);
        }

        public static TheoryData<int, TranslationGroup, bool> TranslationGroupValidationTestData => new()
        {
            { 0, null, true },
            { 1, null, false },
            { 0, TranslationGroupBuilderTests.GetTranslationGroupBuilderWithValidData().Build(), false },
        };

        [Theory]
        [MemberData(nameof(TranslationGroupValidationTestData))]
        public void TranslationGroupValidationTest(int translationGroupId, TranslationGroup translationGroup, bool shouldFail)
            => BuildTranslationGroupTag(builder => builder.SetTranslationGroupId(translationGroupId).SetTranslationGroup(translationGroup), shouldFail);

        public static TheoryData<int, Tag, bool> TagValidationTestData => new()
        {
            { 0, null, true },
            { 1, null, false },
            { 0, TagBuilderTests.GetTagBuilderWithValidData().Build(), false },
        };

        [Theory]
        [MemberData(nameof(TagValidationTestData))]
        public void TagValidationTest(int tagId, Tag tag, bool shouldFail)
            => BuildTranslationGroupTag(builder => builder.SetTagId(tagId).SetTag(tag), shouldFail);

        private static void BuildTranslationGroupTag(Action<TranslationGroupTagBuilder>? builderAction, bool shouldFail)
        {
            var builder = GetTranslationGroupTagBuilderWithValidData();

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

        private static TranslationGroupTagBuilder GetTranslationGroupTagBuilderWithValidData()
        {
            return new TranslationGroupTagBuilder()
                .SetTranslationGroupId(1)
                .SetTagId(1);
        }
    }
}