using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{
    public record KingOfTheHill([property: JsonPropertyName("status")] Dictionary<string, JsonElement> Status)
    {
        public bool IsActive => Status["running"].Deserialize<bool>();
        public bool IsOpen => Status["open"].Deserialize<bool>();
        public TimeSpan TimeToStart => TimeSpan.FromMilliseconds(Status["time_till_start"].Deserialize<int>());
        public TimeSpan MaxCapTime => TimeSpan.FromMilliseconds(Status["cap_time_max"].Deserialize<int>());
        public TimeSpan? CapTime => TimeSpan.FromMilliseconds(Status.GetValueOrDefault("cap_time").Deserialize<int?>() ?? 0);
        public string? CappingName => Status.GetValueOrDefault("capping_name").Deserialize<string?>();
        public string? CappingId => Status.GetValueOrDefault("capping_id").Deserialize<string?>();
    }
}
