using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{
    public record TopIslands([property: JsonPropertyName("top")] List<Island> Islands);
}
