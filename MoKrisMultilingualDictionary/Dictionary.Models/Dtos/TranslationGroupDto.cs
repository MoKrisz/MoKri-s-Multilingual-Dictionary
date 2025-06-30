using System.Text.Json.Serialization;

namespace Dictionary.Models.Dtos
{
    public class TranslationGroupDto
    {
        [JsonPropertyName("translationGroupId")]
        public int TranslationGroupId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<TagDto> Tags { get; set; } = new();
    }
}
