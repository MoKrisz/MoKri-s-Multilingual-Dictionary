using Dictionary.Domain.Validators;

namespace Dictionary.Domain.Builders
{
    public class TranslationGroupTagBuilder : BuilderBase<TranslationGroupTag>
    {
        public TranslationGroupTagBuilder() : this(new TranslationGroupTag())
        { }

        public TranslationGroupTagBuilder(TranslationGroupTag entity) : base(entity, new TranslationGroupTagValidator())
        { }

        public TranslationGroupTagBuilder SetTranslationGroupId(int translationGroupId)
        {
            _entity.TranslationGroupId = translationGroupId;
            return this;
        }

        public TranslationGroupTagBuilder SetTranslationGroup(TranslationGroup translationGroup)
        {
            _entity.TranslationGroup = translationGroup;
            return this;
        }

        public TranslationGroupTagBuilder SetTagId(int tagId)
        {
            _entity.TagId = tagId;
            return this;
        }

        public TranslationGroupTagBuilder SetTag(Tag tag)
        {
            _entity.Tag = tag;
            return this;
        }
    }
}
