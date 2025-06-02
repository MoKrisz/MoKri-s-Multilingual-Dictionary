using Dictionary.Domain.Builders;
using Dictionary.Domain.Enums;

namespace Dictionary.Domain
{
    public class Word
    {
        public int WordId { get; internal set; }
        public string? Article { get; internal set; }
        public string Text { get; internal set; } = string.Empty;
        public string? Plural { get; internal set; }
        public WordTypeEnum Type { get; internal set; }
        public string? Conjugation { get; internal set; }
        public LanguageCodeEnum LanguageCode { get; internal set; }

        public WordBuilder GetBuilder() => new WordBuilder(this);

        internal Word()
        { }
    }
}
