using Dictionary.Domain.Validators;

namespace Dictionary.Domain.Builders
{
    public class WordTranslationGroupBuilder : BuilderBase<WordTranslationGroup>
    {
        public WordTranslationGroupBuilder() : this(new WordTranslationGroup())
        { }

        public WordTranslationGroupBuilder(WordTranslationGroup entity) : base(entity, new WordTranslationGroupValidator())
        { }

        public WordTranslationGroupBuilder SetWordId(int wordId)
        {
            _entity.WordId = wordId;
            return this;
        }

        public WordTranslationGroupBuilder SetWord(Word word)
        {
            _entity.Word = word;
            return this;
        }

        public WordTranslationGroupBuilder SetTranslationGroupId(int translationGroupId)
        {
            _entity.TranslationGroupId = translationGroupId;
            return this;
        }

        public WordTranslationGroupBuilder SetTranslationGroup(TranslationGroup translationGroup)
        {
            _entity.TranslationGroup = translationGroup;
            return this;
        }
    }
}
