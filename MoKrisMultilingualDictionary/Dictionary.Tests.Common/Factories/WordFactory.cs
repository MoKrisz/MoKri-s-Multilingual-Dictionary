using Dictionary.Domain;
using Dictionary.Domain.Builders;

namespace Dictionary.Tests.Common.Factories
{
    public static class WordFactory
    {
        public static Word GermanNoun(
            string article = "die",
            string text = "Mutter",
            string plural = "Mütter")
        {
            return new WordBuilder()
                .SetArticle(article)
                .SetText(text)
                .SetPlural(plural)
                .SetType(Domain.Enums.WordTypeEnum.Noun)
                .SetLanguageCode(Domain.Enums.LanguageCodeEnum.DE)
                .Build();
        }

        public static Word GermanVerb(
            string text = "gehen",
            string conjugation = "gehe/gehst/geht/gehen/geht/gehen")
        {
            return new WordBuilder()
                .SetText(text)
                .SetConjugation(conjugation)
                .SetType(Domain.Enums.WordTypeEnum.Verb)
                .SetLanguageCode(Domain.Enums.LanguageCodeEnum.DE)
                .Build();
        }

        public static Word EnglishNoun(
            string text = "Mother",
            string plural = "Mothers")
        {
            return new WordBuilder()
                .SetText(text)
                .SetPlural(plural)
                .SetType(Domain.Enums.WordTypeEnum.Noun)
                .SetLanguageCode(Domain.Enums.LanguageCodeEnum.EN)
                .Build();
        }
    }
}
