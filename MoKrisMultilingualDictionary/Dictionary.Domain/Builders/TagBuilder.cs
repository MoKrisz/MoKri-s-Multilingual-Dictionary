using Dictionary.Domain.Validators;

namespace Dictionary.Domain.Builders
{
    public class TagBuilder : BuilderBase<Tag>
    {
        public TagBuilder() : this(new Tag())
        { }

        public TagBuilder(Tag entity) : base(entity, new TagValidator())
        { }

        internal TagBuilder SetTagId(int tagId)
        {
            _entity.TagId = tagId;
            return this;
        }

        public TagBuilder SetText(string text)
        {
            _entity.Text = text;
            return this;
        }

        public TagBuilder SetTranslationGroupTags(List<TranslationGroupTag> translationGroupTags)
        {
            _entity.TranslationGroupTags = translationGroupTags;
            return this;
        }
    }
}
