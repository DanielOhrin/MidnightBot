using System;
using System.Collections.Generic;

namespace MidnightBot.Data.BotAPI.Models;

public partial class UserProfile
{
    public long Id { get; set; }

    public string? PlayerId { get; set; }

    public string? ApiKey { get; set; }

    public bool? IsTrackingIsland { get; set; }

    public bool? IsBanned { get; set; }
}
