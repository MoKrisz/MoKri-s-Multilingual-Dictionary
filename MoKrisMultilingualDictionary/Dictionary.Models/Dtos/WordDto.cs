using System.Text.Json.Serialization;

namespace Dictionary.Models.Dtos
{
    public class WordDto
    {
        [JsonPropertyName("wordId")]
        public int WordId { get; set; }

        [JsonPropertyName("article")]
        public string? Article { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("plural")]
        public string? Plural { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("conjugation")]
        public string? Conjugation { get; set; }

        [JsonPropertyName("languageCode")]
        public int LanguageCode { get; set; }
    }
}
