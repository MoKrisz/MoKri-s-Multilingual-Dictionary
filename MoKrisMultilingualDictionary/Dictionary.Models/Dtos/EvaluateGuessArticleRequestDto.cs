using System.Text.Json.Serialization;

namespace Dictionary.Models.Dtos
{
    public class EvaluateGuessArticleRequestDto
    {
        [JsonPropertyName("guesses")]
        public List<EvaluateGuessArticleRequestItemDto> Guesses { get; set; } = new();
    }
}
