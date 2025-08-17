using System.Text.Json.Serialization;

namespace Dictionary.Models.Dtos
{
    public class TranslationDto
    {
        [JsonPropertyName("sourceWordId")]
        public int SourceWordId { get; set; }

        [JsonPropertyName("targetWordId")]
        public int TargetWordId { get; set; }

        [JsonPropertyName("linkedTranslationGroups")]
        public List<TranslationGroupDto> LinkedTranslationGroups { get; set; } = new();
    }
}
