namespace Dictionary.Models.Builders
{
    public abstract class BuilderBase<T> where T : class, new()
    {
        protected T _entity;

        public BuilderBase()
        {
            _entity = new T();
        }

        public T Build()
        {
            return _entity;
        }
    }
}
