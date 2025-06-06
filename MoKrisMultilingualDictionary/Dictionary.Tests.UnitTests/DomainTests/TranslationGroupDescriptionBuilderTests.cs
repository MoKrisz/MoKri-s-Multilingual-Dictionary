using Dictionary.Domain;
using Dictionary.Domain.Builders;
using Dictionary.Domain.Constants;
using FluentValidation;

namespace Dictionary.Tests.UnitTests.DomainTests
{
    public class TranslationGroupDescriptionBuilderTests
    {
        [Fact]
        public void ValidTranslationGroupDescriptionTest()
        {
            BuildTranslationGroupDescription(null, false);
        }

        public static TheoryData<string?, bool> DescriptionValidationTestData => new()
        {
            { null, true },
            { string.Empty, true },
            { "Test", false },
            { new string('a', TranslationGroupDescriptionConstants.DescriptionMaxLength), false },
            { new string('a', TranslationGroupDescriptionConstants.DescriptionMaxLength + 1), true },
        };

        [Theory]
        [MemberData(nameof(DescriptionValidationTestData))]
        public void DescriptionValidationTest(string? description, bool shouldFail)
            => BuildTranslationGroupDescription(builder => builder.SetDescription(description), shouldFail);

        public static TheoryData<int, TranslationGroup, bool> TranslationGroupValidationTestData => new()
        {
            { 0, null, true },
            { 1, null, false },
            { 0, TranslationGroupBuilderTests.GetTranslationGroupBuilderWithValidData().Build(), false },
        };

        [Theory]
        [MemberData(nameof(TranslationGroupValidationTestData))]
        public void TranslationGroupValidationTest(int translationGroupId, TranslationGroup translationGroup, bool shouldFail)
            => BuildTranslationGroupDescription(builder => builder.SetTranslationGroupId(translationGroupId).SetTranslationGroup(translationGroup), shouldFail);

        private static void BuildTranslationGroupDescription(Action<TranslationGroupDescriptionBuilder>? builderAction, bool shouldFail)
        {
            var builder = GetTranslationGroupDescriptionBuilderWithValidData();

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

        public static TranslationGroupDescriptionBuilder GetTranslationGroupDescriptionBuilderWithValidData()
        {
            return new TranslationGroupDescriptionBuilder()
                .SetDescription("Test")
                .SetTranslationGroupId(1);
        }
    }
}