namespace Dictionary.Models.Builders
{
    public abstract class BuilderBase<T> where T : class
    {
        protected T _entity;

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
