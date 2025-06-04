using Dictionary.Domain.Validators;

namespace Dictionary.Domain.Builders
{
    public class TranslationGroupDescriptionBuilder : BuilderBase<TranslationGroupDescription>
    {
        public TranslationGroupDescriptionBuilder() : this(new TranslationGroupDescription())
        { }

        public TranslationGroupDescriptionBuilder(TranslationGroupDescription entity) : base(entity, new TranslationGroupDescriptionValidator())
        { }

        public TranslationGroupDescriptionBuilder SetDescription(string description)
        {
            _entity.Description = description;
            return this;
        }

        public TranslationGroupDescriptionBuilder SetTranslationGroupId(int translationGroupId)
        {
            _entity.TranslationGroupId = translationGroupId;
            return this;
        }

        public TranslationGroupDescriptionBuilder SetTranslationGroup(TranslationGroup translationGroup)
        {
            _entity.TranslationGroup = translationGroup;
            return this;
        }
    }
}
