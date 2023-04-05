using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{
    public record Server([property: JsonPropertyName("servers")] int ServerCount, [property: JsonPropertyName("proxies")] int ProxyCount, [property: JsonPropertyName("players")] int PlayerCount);
}
