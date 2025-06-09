using Dictionary.Domain.Builders;
using Dictionary.Domain.Constants;
using FluentValidation;

namespace Dictionary.Tests.UnitTests.DomainTests
{
    public class TranslationGroupBuilderTests
    {
        [Fact]
        public void ValidTranslationGroupDescriptionTest()
        {
            BuildTranslationGroup(null, false);
        }

        public static TheoryData<string?, bool> DescriptionValidationTestData => new()
        {
            { null, true },
            { string.Empty, true },
            { "Test", false },
            { new string('a', TranslationGroupConstants.DescriptionMaxLength), false },
            { new string('a', TranslationGroupConstants.DescriptionMaxLength + 1), true },
        };

        [Theory]
        [MemberData(nameof(DescriptionValidationTestData))]
        public void DescriptionValidationTest(string? description, bool shouldFail)
            => BuildTranslationGroup(builder => builder.SetDescription(description), shouldFail);

        private static void BuildTranslationGroup(Action<TranslationGroupBuilder>? builderAction, bool shouldFail)
        {
            var builder = GetTranslationGroupBuilderWithValidData();

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

        public static TranslationGroupBuilder GetTranslationGroupBuilderWithValidData()
        {
            return new TranslationGroupBuilder().SetDescription("Test");
        }
    }
}