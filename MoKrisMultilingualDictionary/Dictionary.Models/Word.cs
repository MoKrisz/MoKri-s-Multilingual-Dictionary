﻿using Dictionary.Models.Enums;
using Dictionary.Validations;

namespace Dictionary.Models
{
    public partial class Word
    {
        public int WordId { get; private set; }
        public string? Article { get; private set; }
        public string Text { get; private set; } = string.Empty;
        public string? Plural { get; private set; }
        public WordTypeEnum Type { get; private set; }
        public string? Conjugation { get; private set; }
        public LanguageCodeEnum LanguageCode { get; private set; }

        public WordBuilder GetBuilder()
        {
            return new WordBuilder(this);
        }

        private Word() 
        { }

        public class WordBuilder : BuilderBase<Word>
        {
            public WordBuilder() : this(new Word())
            { }

            public WordBuilder(Word entity) : base(entity, new WordValidator())
            { }

            public WordBuilder SetArticle(string? article)
            {
                _entity.Article = article;
                return this;
            }

            public WordBuilder SetText(string text)
            {
                _entity.Text = text;
                return this;
            }

            public WordBuilder SetPlural(string? plural)
            {
                _entity.Plural = plural;
                return this;
            }

            public WordBuilder SetType(WordTypeEnum type)
            {
                _entity.Type = type;
                return this;
            }

            public WordBuilder SetConjugation(string? conjugation)
            {
                _entity.Conjugation = conjugation;
                return this;
            }

            public WordBuilder SetLanguageCode(LanguageCodeEnum languageCode)
            {
                _entity.LanguageCode = languageCode;
                return this;
            }
        }
    }
}
