using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{
    public record PlayerWithAlliance([property: JsonPropertyName("id")] string Id, [property: JsonPropertyName("name")] string Name, [property: JsonPropertyName("role")] string Role);
}
