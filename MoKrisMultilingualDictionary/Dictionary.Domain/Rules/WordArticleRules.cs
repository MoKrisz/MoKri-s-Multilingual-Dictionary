using Dictionary.Domain.Enums;

namespace Dictionary.Domain.Rules
{
    public static class WordArticleRules
    {
        public static readonly Dictionary<LanguageCodeEnum, HashSet<string>> ValidArticlesByLanguage =
            new()
            {
                { LanguageCodeEnum.DE, ["der", "die", "das"] },
                { LanguageCodeEnum.HU, ["a", "az"] }
            };
    }
}
