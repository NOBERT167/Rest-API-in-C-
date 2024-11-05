using System.Text.Json.Serialization;

namespace BusinessCentralApi.Models
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("status")]
        public required string Status { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("data")]
        public required T Data { get; set; }

        [JsonPropertyName("message")]
        public required string Message { get; set; }

        [JsonPropertyName("errorDetails")]
        public required string ErrorDetails { get; set; }
    }
}