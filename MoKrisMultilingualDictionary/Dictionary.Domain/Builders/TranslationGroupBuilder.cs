using Dictionary.Domain.Validators;

namespace Dictionary.Domain.Builders
{
    public class TranslationGroupBuilder : BuilderBase<TranslationGroup>
    {
        public TranslationGroupBuilder() : this(new TranslationGroup())
        { }

        public TranslationGroupBuilder(TranslationGroup entity) : base(entity, new TranslationGroupValidator())
        { }

        public TranslationGroupBuilder SetDescription(string description)
        {
            _entity.Description = description;
            return this;
        }

        public TranslationGroupBuilder SetWordTranslationGroups(List<WordTranslationGroup> wordTranslationGroups)
        {
            _entity.WordTranslationGroups = wordTranslationGroups;
            return this;
        }

        public TranslationGroupBuilder SetTranslationGroupTags(List<TranslationGroupTag> translationGroupTags)
        {
            _entity.TranslationGroupTags = translationGroupTags;
            return this;
        }
    }
}
