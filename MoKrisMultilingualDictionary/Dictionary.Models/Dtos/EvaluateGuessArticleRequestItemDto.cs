using System.Text.Json.Serialization;

namespace Dictionary.Models.Dtos
{
    public class EvaluateGuessArticleRequestItemDto
    {
        [JsonPropertyName("wordId")]
        public int WordId { get; set; }

        [JsonPropertyName("answer")]
        public string Answer { get; set; } = string.Empty;
    }
}
