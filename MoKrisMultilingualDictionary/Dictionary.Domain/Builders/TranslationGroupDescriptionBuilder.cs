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
    }
}
