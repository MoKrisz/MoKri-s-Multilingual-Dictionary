namespace Dictionary.BusinessLogic.Services.Synchronization
{
    public interface IDataSynchronizerComparer<TSource, TTarget, TKey>
        where TKey : struct
    {
        TKey GetSourceKey(TSource source);
        TKey? GetTargetKey(TTarget target);
        bool EqualityCompare(TSource source, TTarget target);
    }
}
