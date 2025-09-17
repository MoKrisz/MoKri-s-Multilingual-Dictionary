using Dictionary.Models.Dtos;

namespace Dictionary.BusinessLogic.Abstractions.Tag
{
    public interface ITagService
    {
        Task<List<Domain.Tag>> GetOrCreateTagsAsync(List<TagDto> tagDtos, CancellationToken cancellationToken);
    }
}
