namespace Dictionary.BusinessLogic.Services.Synchronization
{
    public class DataSynchronizer : IDataSynchronizer
    {
        public DataSynchronizerResult<TSource, TTarget> Synchronize<TSource, TTarget, TKey>(IEnumerable<TSource> source, IEnumerable<TTarget> target, IDataSynchronizerComparer<TSource, TTarget, TKey> comparer)
            where TKey : struct
        {
            var added = new List<TTarget>();
            var modified = new List<ModifiedItem<TSource, TTarget>>();

            var sourceDictionary = source.ToDictionary(comparer.GetSourceKey);

            foreach (var targetItem in target)
            {
                var targetKey = comparer.GetTargetKey(targetItem);

                if (targetKey != null && sourceDictionary.TryGetValue(targetKey.Value, out var sourceItem))
                {
                    if (!comparer.EqualityCompare(sourceItem, targetItem))
                    {
                        modified.Add(new ModifiedItem<TSource, TTarget>
                        {
                            Source = sourceItem,
                            Target = targetItem,
                        });

                        sourceDictionary.Remove(targetKey.Value);
                    }
                }
                else 
                {
                    added.Add(targetItem);
                }
            }

            var deleted = sourceDictionary.Values.ToList();

            return new DataSynchronizerResult<TSource, TTarget>()
            {
                Added = added,
                Modified = modified,
                Deleted = deleted
            };
        }
    }
}
