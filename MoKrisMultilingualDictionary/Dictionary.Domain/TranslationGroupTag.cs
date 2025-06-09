using Dictionary.Domain.Builders;

namespace Dictionary.Domain
{
    public class TranslationGroupTag
    {
        public int TranslationGroupTagId { get; internal set; }
        public int TranslationGroupId { get; internal set; }
        public TranslationGroup TranslationGroup { get; internal set; } = default!;
        public int TagId { get; internal set; }
        public Tag Tag { get; internal set; } = default!;

        public TranslationGroupTagBuilder GetBuilder() => new TranslationGroupTagBuilder(this);

        internal TranslationGroupTag()
        { }
    }
}
