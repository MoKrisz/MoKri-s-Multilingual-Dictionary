using System.Text.Json.Serialization;

namespace Dictionary.Models.Dtos
{
    public class EvaluateGuessArticleResponseItemDto
    {
        [JsonPropertyName("wordId")]
        public int WordId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("answer")]
        public string Answer { get; set; } = string.Empty;

        [JsonPropertyName("correctArticle")]
        public string CorrectArticle { get; set; } = string.Empty;

        [JsonPropertyName("isCorrect")]
        public bool IsCorrect { get; set; }
    }
}
