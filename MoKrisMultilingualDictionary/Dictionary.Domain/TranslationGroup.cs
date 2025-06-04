using Dictionary.Domain.Builders;

namespace Dictionary.Domain
{
    public class TranslationGroup
    {
        public int TranslationGroupId { get; internal set; }
        public int TranslationGroupDescriptionId { get; internal set; }
        public TranslationGroupDescription TranslationGroupDescription { get; internal set; } = default!;
        public List<TranslationGroupTag> TranslationGroupTags { get; internal set; } = new();
        public List<WordTranslationGroup> WordTranslationGroups { get; internal set; } = new();

        public TranslationGroupBuilder GetBuilder() => new TranslationGroupBuilder(this);

        internal TranslationGroup()
        { }
    }
}
