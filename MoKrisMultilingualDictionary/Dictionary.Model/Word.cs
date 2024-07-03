using Dictionary.Models.Enums;

namespace Dictionary.Models
{
    public class Word
    {
        public int WordId { get; set; }
        public string? Article { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? Plural { get; set; }
        public WordTypeEnum Type { get; set; }
        public string? Conjugation { get; set; }
        public LanguageCodeEnum LanguageCode { get; set; }
    }
}
