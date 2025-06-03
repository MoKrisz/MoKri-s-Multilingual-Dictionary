using Dictionary.Domain.Validators;

namespace Dictionary.Domain.Builders
{
    public class TagBuilder : BuilderBase<Tag>
    {
        public TagBuilder() : this(new Tag())
        { }

        public TagBuilder(Tag entity) : base(entity, new TagValidator())
        { }

        public TagBuilder SetText(string text)
        {
            _entity.Text = text;
            return this;
        }
    }
}
