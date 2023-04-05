using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{
    public record PlayerWithEconomy([property: JsonPropertyName("account_id")] string Id, [property: JsonPropertyName("account_name")] string Name, [property: JsonPropertyName("leaderboard_position")] int Position, [property: JsonPropertyName("balance")] string Balance);
}
