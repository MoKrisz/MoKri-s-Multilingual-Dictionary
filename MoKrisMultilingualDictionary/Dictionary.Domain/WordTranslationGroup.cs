using Dictionary.Domain.Builders;

namespace Dictionary.Domain
{
    public class WordTranslationGroup
    {
        public int WordTranslationGroupId { get; internal set; }
        public int WordId { get; internal set; }
        public Word Word { get; internal set; } = default!;
        public int TranslationGroupId { get; internal set; }
        public TranslationGroup TranslationGroup { get; internal set; } = default!;

        public WordTranslationGroupBuilder GetBuilder() => new WordTranslationGroupBuilder(this);

        internal WordTranslationGroup()
        { }
    }
}
