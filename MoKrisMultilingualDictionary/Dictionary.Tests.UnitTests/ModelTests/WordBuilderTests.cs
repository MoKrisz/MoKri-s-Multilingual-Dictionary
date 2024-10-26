using Dictionary.Models.Enums;
using FluentValidation;
using static Dictionary.Models.Word;

namespace Dictionary.Tests.UnitTests.ModelTests
{
    public class WordBuilderTests
    {
        [Fact]
        public void ValidWordTest()
        {
            BuildWord(null, false);
        }

        public static TheoryData<string?, LanguageCodeEnum, WordTypeEnum, bool> ArticleValidationTestData => new()
        {
            { null, LanguageCodeEnum.EN, WordTypeEnum.Noun, false },
            { null, LanguageCodeEnum.HU, WordTypeEnum.Noun, false },
            { "der", LanguageCodeEnum.DE, WordTypeEnum.Noun, false },
            { "der", LanguageCodeEnum.DE, WordTypeEnum.Verb, true },
            { "der", LanguageCodeEnum.DE, WordTypeEnum.Adjective, true },
            { "der", LanguageCodeEnum.EN, WordTypeEnum.Noun, true },
            { "der", LanguageCodeEnum.HU, WordTypeEnum.Noun, true },
        };

        [Theory]
        [MemberData(nameof(ArticleValidationTestData))]
        public void ArticleValidationTest(string? article, LanguageCodeEnum language, WordTypeEnum type, bool shouldFail)
        {
            var builderAction = (WordBuilder builder) => {
                builder.SetArticle(article)
                    .SetLanguageCode(language)
                    .SetType(type);

                if (type == WordTypeEnum.Verb)
                {
                    builder.SetConjugation("invalid_conjugation_just_for_testing");
                }
                
                if (type != WordTypeEnum.Noun)
                {
                    builder.SetPlural(string.Empty);
                }
            };

            BuildWord(builderAction, shouldFail);
        }

        public static TheoryData<string?, bool> WordTextValidationTestData => new()
        {
            { null, true },
            { string.Empty, true },
            { "Test", false },
        };

        [Theory]
        [MemberData(nameof(WordTextValidationTestData))]
        public void WordTextValidationTest(string? text, bool shouldFail)
        {
            BuildWord(builder => builder.SetText(text), shouldFail);
        }

        public static TheoryData<string?, WordTypeEnum, bool> PluralValidationTestData => new()
        {
            { null, WordTypeEnum.Adjective, false },
            { string.Empty, WordTypeEnum.Verb, false },
            { "Tests", WordTypeEnum.Noun, false },
            { string.Empty, WordTypeEnum.Noun, true },
        };

        [Theory]
        [MemberData(nameof(PluralValidationTestData))]
        public void PluralValidationTest(string? plural, WordTypeEnum type, bool shouldFail)
        {
            var builderAction = (WordBuilder builder) => 
            {
                builder.SetPlural(plural)
                    .SetType(type);

                if (type == WordTypeEnum.Verb)
                {
                    builder.SetConjugation("invalid_conjugation_just_for_testing");
                }
            };

            BuildWord(builderAction, shouldFail);
        }

        public static TheoryData<WordTypeEnum, bool> TypeValidationTestData => new()
        {
            { WordTypeEnum.Adjective, false },
            { WordTypeEnum.None, true },
        };

        [Theory]
        [MemberData(nameof(TypeValidationTestData))]
        public void TypeValidationTest(WordTypeEnum type, bool shouldFail)
        {
            BuildWord(builder => builder.SetType(type).SetPlural(string.Empty), shouldFail);
        }

        public static TheoryData<string?, WordTypeEnum, bool> ConjugationValidationTestData => new()
        {
            { "invalid_conjugation_just_for_testing", WordTypeEnum.Verb, false },
            { null, WordTypeEnum.Noun, false },
            { string.Empty, WordTypeEnum.Adjective, false },
            { string.Empty, WordTypeEnum.Verb, true },
            { "invalid_conjugation_just_for_testing", WordTypeEnum.Noun, true },
            { "invalid_conjugation_just_for_testing", WordTypeEnum.Adjective, true },
        };

        [Theory]
        [MemberData(nameof(ConjugationValidationTestData))]
        public void ConjugationValidationTest(string conjugation, WordTypeEnum type, bool shouldFail)
        {
            var builderAction = (WordBuilder builder) =>
            {
                builder.SetConjugation(conjugation)
                    .SetType(type);

                if (type != WordTypeEnum.Noun)
                {
                    builder.SetPlural(string.Empty);
                }
            };

            BuildWord(builderAction, shouldFail);
        }

        public static TheoryData<LanguageCodeEnum, bool> LanguageValidationTestData => new()
        {
            {LanguageCodeEnum.EN, false },
            {LanguageCodeEnum.None, true },
        };

        [Theory]
        [MemberData(nameof(LanguageValidationTestData))]
        public void LanguageValidationTest(LanguageCodeEnum language, bool shouldFail)
        {
            BuildWord(builder => builder.SetLanguageCode(language), shouldFail);
        }

        private static void BuildWord(Action<WordBuilder>? builderAction, bool shouldFail)
        {
            var builder = GetWordBuilderWithValidData();

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

        private static WordBuilder GetWordBuilderWithValidData()
        {
            return new WordBuilder()
                .SetArticle(null)
                .SetText("Test")
                .SetPlural("Tests")
                .SetType(WordTypeEnum.Noun)
                .SetConjugation(null)
                .SetLanguageCode(LanguageCodeEnum.EN);
        }
    }
}