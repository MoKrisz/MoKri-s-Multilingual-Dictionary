namespace Dictionary.BusinessLogic.Services.Synchronization
{
    public interface IDataSynchronizer
    {
        DataSynchronizerResult<TSource, TTarget> Synchronize<TSource, TTarget, TKey>(IEnumerable<TSource> source, IEnumerable<TTarget> target, IDataSynchronizerComparer<TSource, TTarget, TKey> comparer)
            where TKey : struct;
    }
}
