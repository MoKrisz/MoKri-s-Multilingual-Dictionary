using System.Text.Json.Serialization;

namespace Dictionary.Models.Dtos
{
    public class WordRelatedTranslationGroupsDto
    {
        [JsonPropertyName("potentialTranslationGroups")]
        public List<TranslationGroupDto> PotentialTranslationGroups { get; set; } = new();

        [JsonPropertyName("linkedTranslationGroups")]
        public List<TranslationGroupDto> LinkedTranslationGroups { get; set; } = new();
    }
}
