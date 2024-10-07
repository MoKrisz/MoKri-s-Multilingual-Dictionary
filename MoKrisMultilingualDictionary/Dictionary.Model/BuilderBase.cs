using FluentValidation;

namespace Dictionary.Models
{
    public abstract class BuilderBase<T> where T : class
    {
        protected T _entity;
        private readonly IValidator<T> _validator;

        public BuilderBase(T entity, IValidator<T> validator)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public T Build()
        {
            return _entity;
        }
    }
}
