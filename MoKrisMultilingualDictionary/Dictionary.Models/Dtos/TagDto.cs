using System.Text.Json.Serialization;

namespace Dictionary.Models.Dtos
{
    public class TagDto
    {
        [JsonPropertyName("tagId")]
        public int? TagId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
