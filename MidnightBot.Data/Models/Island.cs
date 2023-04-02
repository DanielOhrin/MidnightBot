using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{ 
    public record Island([property: JsonPropertyName("island_id")] string Id, [property: JsonPropertyName("island_name")] string? Name, [property: JsonPropertyName("hourly_xp")] int Amount);
}
