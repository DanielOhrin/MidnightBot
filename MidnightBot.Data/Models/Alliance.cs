using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{
    public record Alliance ([property: JsonPropertyName("name")] string Name, [property: JsonPropertyName("id")] string Id, [property: JsonPropertyName("relations")] Dictionary<string, string> Relations, [property: JsonPropertyName("members")] List<PlayerWithAlliance> Members);
}
