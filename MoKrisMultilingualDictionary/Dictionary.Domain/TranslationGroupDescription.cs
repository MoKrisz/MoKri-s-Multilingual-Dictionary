using Dictionary.Domain.Builders;

namespace Dictionary.Domain
{
    public class TranslationGroupDescription
    {
        public int TranslationGroupDescriptionId { get; internal set; }
        public string Description { get; internal set; } = string.Empty;
        public int TranslationGroupId { get; internal set; }
        public TranslationGroup TranslationGroup { get; internal set; } = default!;

        public TranslationGroupDescriptionBuilder GetBuilder() => new TranslationGroupDescriptionBuilder(this);

        internal TranslationGroupDescription()
        { }
    }
}
