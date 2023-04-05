using System.Text.Json.Serialization;

namespace MidnightBot.Data.Models
{
    public record BlackAuctionHouse([property: JsonPropertyName("start_time")] long StartTime, [property: JsonPropertyName("end_time")] long EndTime, [property: JsonPropertyName("current_bid")] string CurrentBid, [property: JsonPropertyName("next_bid")] string NextBid, [property: JsonPropertyName("current_bidder")] string BidderId, [property: JsonPropertyName("current_bidder_name")] string BidderName);
}
