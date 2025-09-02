namespace Dictionary.BusinessLogic.Services.Synchronization
{
    public class DataSynchronizerResult<TSource, TTarget>
    {
        public IReadOnlyList<TTarget> Added { get; init; } = new List<TTarget>();

        public IReadOnlyList<ModifiedItem<TSource, TTarget>> Modified { get; init; } = new List<ModifiedItem<TSource, TTarget>>();

        public IReadOnlyList<TSource> Deleted { get; init; } = new List<TSource>();
    }

    public class ModifiedItem<TSource, TTarget>
    {
        public TSource Source { get; init; } = default!;
        public TTarget Target { get; init; } = default!;
    }
}
