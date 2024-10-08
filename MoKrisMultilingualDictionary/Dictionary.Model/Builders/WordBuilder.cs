﻿using Dictionary.Models.Enums;

namespace Dictionary.Models.Builders
{
    public class WordBuilder : BuilderBase<Word>
    {
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
