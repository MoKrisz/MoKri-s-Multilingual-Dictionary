using System.Text.Json.Serialization;

namespace Dictionary.Tests.IntegrationTests.WebApi.Common
{
    public class ODataResponse<T>
    {
        [JsonPropertyName("@odata.count")]
        public int Count { get; set; }

        [JsonPropertyName("value")]
        public List<T> Values { get; set; } = default!;
    }
}
