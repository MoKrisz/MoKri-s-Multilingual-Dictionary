using FluentValidation;

namespace Dictionary.Models.Builders
{
    public abstract class BuilderBase<T> where T : class
    {
        protected T _entity;
        private readonly IValidator<T> _validator;

        //Factory should be used here to create the validator.
        public BuilderBase(T entity)
        {
            _entity = entity;
        }

        public T Build()
        {
            return _entity;
        }
    }
}
