using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;

namespace MidnightBot.SlashCommands
{
    public class DailyResetCommands : ApplicationCommandModule
    {
        [SlashCommand("quotas", "Shows how much time until farming quotas reset.")]
        public static async Task QuotasCommand(InteractionContext ctx)
        {
            DateTime currentDate = DateTime.Now;
            TimeSpan timeUntilQuotaResets;

            if (currentDate.TimeOfDay.Hours > 12 && currentDate.TimeOfDay.Seconds > 0)
            {
                timeUntilQuotaResets = DateTime.Today.AddDays(1).AddHours(13) - currentDate;
            }
            else
            {
                timeUntilQuotaResets = DateTime.Today.AddHours(13) - currentDate;
            }

            string response = $"**Farming Quotas reset in {timeUntilQuotaResets.ToString(@"hh'h 'mm'm 'ss's'")}**";    
           
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(response));
        }

        [SlashCommand("levelcap", "Shows how much time until levelcap increases.")]
        public static async Task LevelcapCommand(InteractionContext ctx)
        {
            DateTime currentDate = DateTime.Now;
            TimeSpan timeUntilLevelcapResets = DateTime.Today.AddDays(1) - currentDate;

            string response = $"**Levelcap increases in {timeUntilLevelcapResets.ToString(@"hh'h 'mm'm 'ss's'")}**";

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(response));
        }
    }
}
