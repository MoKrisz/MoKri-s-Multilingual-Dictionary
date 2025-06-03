using Dictionary.Domain.Validators;

namespace Dictionary.Domain.Builders
{
    public class TranslationGroupBuilder : BuilderBase<TranslationGroup>
    {
        public TranslationGroupBuilder() : this(new TranslationGroup())
        { }

        public TranslationGroupBuilder(TranslationGroup entity) : base(entity, new TranslationGroupValidator())
        { }

        public TranslationGroupBuilder SetTranslationGroupDescriptionId(int translationGroupDescriptionId)
        {
            _entity.TranslationGroupDescriptionId = translationGroupDescriptionId;
            return this;
        }

        public TranslationGroupBuilder SetTranslationGroupDescription(TranslationGroupDescription translationGroupDescription)
        {
            _entity.TranslationGroupDescription = translationGroupDescription;
            return this;
        }

        public TranslationGroupBuilder SetWordTranslationGroups(List<WordTranslationGroup> wordTranslationGroups)
        {
            _entity.WordTranslationGroups = wordTranslationGroups;
            return this;
        }
    }
}
