using System.ComponentModel;

namespace MidnightBot.Enums
{
    public enum GetAllianceSearchTypeEnum
    {
        [Description("by_player_name")]
        PlayerName = 1,
        [Description("by_player_id")]
        PlayerId = 2,
        [Description("by_name")]
        AllianceName = 3,
        [Description("by_id")]
        AllianceId = 4
    }
}
