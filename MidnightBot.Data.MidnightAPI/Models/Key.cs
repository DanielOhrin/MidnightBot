using System.Text.Json.Serialization;

namespace MidnightBot.Data.MidnightAPI.Models
{
    public record Key([property: JsonPropertyName("player")] string PlayerId, [property: JsonPropertyName("rate_limit_per_minute")] int CallsPerMinute, [property: JsonPropertyName("level")] string Level);
}
