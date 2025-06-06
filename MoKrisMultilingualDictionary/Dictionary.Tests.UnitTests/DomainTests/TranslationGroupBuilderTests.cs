using Dictionary.Domain;
using Dictionary.Domain.Builders;
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

        public static TheoryData<int, TranslationGroupDescription, bool> TranslationGroupDescriptionValidationTestData => new()
        {
            { 0, null, true },
            { 1, null, false },
            { 0, TranslationGroupDescriptionBuilderTests.GetTranslationGroupDescriptionBuilderWithValidData().Build(), false },
        };

        [Theory]
        [MemberData(nameof(TranslationGroupDescriptionValidationTestData))]
        public void TranslationGroupDescriptionValidationTest(int translationGroupDescriptionId, TranslationGroupDescription translationGroupDescription, bool shouldFail)
        {
            BuildTranslationGroup(builder => builder.SetTranslationGroupDescriptionId(translationGroupDescriptionId)
                                                .SetTranslationGroupDescription(translationGroupDescription), shouldFail);
        }

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
            return new TranslationGroupBuilder().SetTranslationGroupDescriptionId(1);
        }
    }
}