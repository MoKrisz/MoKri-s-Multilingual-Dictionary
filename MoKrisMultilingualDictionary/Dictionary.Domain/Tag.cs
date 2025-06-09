using Dictionary.Domain.Builders;

namespace Dictionary.Domain
{
    public class Tag
    {
        public int TagId { get; internal set; }
        public string Text { get; internal set; } = string.Empty;
        public List<TranslationGroupTag> TranslationGroupTags { get; internal set; } = new();

        public TagBuilder GetBuilder() => new TagBuilder(this);

        internal Tag()
        { }
    }
}
