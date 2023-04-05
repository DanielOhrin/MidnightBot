using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{
    public record TopTenPlayerWithEconomy([property: JsonPropertyName("top")] List<PlayerWithEconomy> Players);
}
