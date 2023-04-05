using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{//TODO Add the remaining properties once we can sit in LMS to see example data.
    public record LastManStanding([property: JsonPropertyName("running")] bool IsActive);
}
