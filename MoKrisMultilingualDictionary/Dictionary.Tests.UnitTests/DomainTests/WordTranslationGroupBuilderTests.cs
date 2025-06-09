using Dictionary.Domain;
using Dictionary.Domain.Builders;
using FluentValidation;

namespace Dictionary.Tests.UnitTests.DomainTests
{
    public class WordTranslationGroupBuilderTests
    {
        [Fact]
        public void ValidWordTranslationGroupTest()
        {
            BuildWordTranslationGroupTag(null, false);
        }

        public static TheoryData<int, Word, bool> WordValidationTestData => new()
        {
            { 0, null, true },
            { 1, null, false },
            { 0, WordBuilderTests.GetWordBuilderWithValidData().Build(), false },
        };

        [Theory]
        [MemberData(nameof(WordValidationTestData))]
        public void WordValidationTest(int wordId, Word word, bool shouldFail)
            => BuildWordTranslationGroupTag(builder => builder.SetWordId(wordId).SetWord(word), shouldFail);

        public static TheoryData<int, TranslationGroup, bool> TranslationGroupValidationTestData => new()
        {
            { 0, null, true },
            { 1, null, false },
            { 0, TranslationGroupBuilderTests.GetTranslationGroupBuilderWithValidData().Build(), false },
        };

        [Theory]
        [MemberData(nameof(TranslationGroupValidationTestData))]
        public void TranslationGroupValidationTest(int translationGroupId, TranslationGroup translationGroup, bool shouldFail)
            => BuildWordTranslationGroupTag(builder => builder.SetTranslationGroupId(translationGroupId).SetTranslationGroup(translationGroup), shouldFail);

        private static void BuildWordTranslationGroupTag(Action<WordTranslationGroupBuilder>? builderAction, bool shouldFail)
        {
            var builder = GetWordTranslationGroupBuilderWithValidData();

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

        private static WordTranslationGroupBuilder GetWordTranslationGroupBuilderWithValidData()
        {
            return new WordTranslationGroupBuilder()
                .SetWordId(1)
                .SetTranslationGroupId(1);
        }
    }
}