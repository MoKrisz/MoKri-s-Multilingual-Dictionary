using Dictionary.BusinessLogic.Services.Synchronization;
using Dictionary.Models.Dtos;

namespace Dictionary.BusinessLogic.Tag
{
    public class TagComparer : IDataSynchronizerComparer<Domain.Tag, TagDto, int>
    {
        public bool EqualityCompare(Domain.Tag source, TagDto target)
        {
            return source.Text.Equals(target.Text, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetSourceKey(Domain.Tag source)
        {
            return source.TagId;
        }

        public int? GetTargetKey(TagDto target)
        {
            return target.TagId;
        }
    }
}
