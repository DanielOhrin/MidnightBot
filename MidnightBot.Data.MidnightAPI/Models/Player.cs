using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{
    public record Player([property: JsonPropertyName("id")] string Id, [property: JsonPropertyName("name")] string Name);
}
