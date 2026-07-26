using System.Text.Json.Serialization;

namespace Dictionary.Models.Dtos
{
    public class GuessArticleWordDto
    {
        [JsonPropertyName("wordId")]
        public int WordId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
