using Dictionary.Models.Builders;
using Dictionary.Models.Enums;

namespace Dictionary.Models
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

        public WordBuilder GetBuilder()
        {
            return new WordBuilder(this);
        }
    }
}
