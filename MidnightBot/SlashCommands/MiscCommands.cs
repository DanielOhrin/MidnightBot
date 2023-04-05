using System.Text;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using MidnightBot.Data.API;
using MidnightBot.Data.Models;
using MidnightBot.Services;

namespace MidnightBot.SlashCommands
{
    public class MiscCommands : ApplicationCommandModule
    {
        [SlashCommand("daily", "Returns the amount of time until the next occurance of each daily reset.")]
        public static async Task QuotasCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            DateTime currentDate = DateTime.Now;
            MidnightEmbedBuilder embed = new();

            //! Farming quotas
            TimeSpan timeUntilQuotaResets;

            if (currentDate.TimeOfDay.Hours > 12 && currentDate.TimeOfDay.Seconds > 0)
            {
                timeUntilQuotaResets = DateTime.Today.AddDays(1).AddHours(13) - currentDate;
            }
            else
            {
                timeUntilQuotaResets = DateTime.Today.AddHours(13) - currentDate;
            }

            embed.AddField("**Farming Quotas**", timeUntilQuotaResets.ToString(@"hh'h 'mm'm 'ss's'"));

            //! Realms
            TimeSpan timeUntilRealmsReset;

            if (currentDate.TimeOfDay.Hours > 13 && currentDate.TimeOfDay.Seconds > 0)
            {
                timeUntilRealmsReset = DateTime.Today.AddDays(1).AddHours(14) - currentDate;
            }
            else
            {
                timeUntilRealmsReset = DateTime.Today.AddHours(14) - currentDate;
            }

            embed.AddField("**Realms**", timeUntilRealmsReset.ToString(@"hh'h 'mm'm 'ss's'"));

            //! Levelcap
            TimeSpan timeUntilLevelcapResets = DateTime.Today.AddDays(1) - currentDate;
            embed.AddField("**Levelcap **", timeUntilLevelcapResets.ToString(@"hh'h 'mm'm 'ss's'"));

            embed.WithTitle("Daily Reset");
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("koth", "Returns all available information about the KOTH event.")]
        public async Task KothCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new();
            try
            {

                KingOfTheHill? koth = await MidnightAPI.GetKingOfTheHillAsync();

                StringBuilder sb = new();

                sb.AppendLine($"**Active »** {koth.IsActive.ToString()[..1].ToUpper() + koth.IsActive.ToString()[1..]}");
                sb.AppendLine($"**Open »** {koth.IsOpen.ToString()[..1].ToUpper() + koth.IsOpen.ToString()[1..]}");
                sb.AppendLine();

                if (koth.IsActive)
                {
                    //! Show information about the current KoTH
                    sb.AppendLine($"**Player capturing »** {koth.CappingName ?? "N/A"}");
                    sb.AppendLine($"**Time until captured »** {(koth.CapTime != null ? $"{koth.CapTime?.Minutes}:{koth.CapTime?.Seconds}" : "15:00")}");
                }
                else
                {
                    //! Show information about the upcoming KoTH
                    sb.AppendLine($"**Starts in**");
                    sb.AppendLine($"{koth.TimeToStart.Hours}h {koth.TimeToStart.Minutes}m {koth.TimeToStart.Seconds}s");
                }

                embed.WithTitle("KOTH").WithDescription(sb.ToString());
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
            }
            catch(Exception ex)
            {
                embed.UnknownError();
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                Console.WriteLine(ex.ToString());
            }
        }
        
        [SlashCommand("bah", "Returns all available information about the BAH.")]
        public async Task BahCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new();
            try
            {

                BlackAuctionHouse? bah = await MidnightAPI.GetBlackAuctionHouseAsync();

                StringBuilder sb = new();

                sb.AppendLine($"**Active »** {(bah.StartTime == 0 ? "True" : "False")}");
                sb.AppendLine();
              
                if (bah.StartTime == 0)
                {
                    //! Show information about the current BAH auction
                    sb.AppendLine($"**Player bidding** » {bah.BidderName}");
                    sb.AppendLine();

                    sb.AppendLine($"**Current bid »** {BotUtils.FormatNumber(bah.CurrentBid)}");
                    sb.AppendLine($"**Next bid »** {BotUtils.FormatNumber(bah.NextBid)}");
                    sb.AppendLine();

                    sb.AppendLine("**Ends in**");
                    TimeSpan timeUntilEnd = DateTime.UnixEpoch.AddMilliseconds(bah.EndTime) - DateTime.Now;
                    sb.AppendLine($"{timeUntilEnd.Minutes}m {timeUntilEnd.Seconds}s");
                }
                else
                {
                    //! Show information about the upcoming BAH auction
                    sb.AppendLine($"**Starts in**");
                    TimeSpan timeUntilStart = DateTime.UnixEpoch.AddMilliseconds(bah.StartTime) - DateTime.Now;
                    sb.AppendLine($"{timeUntilStart.Minutes}m {timeUntilStart.Seconds}s");
                }

                embed.WithTitle("BAH").WithDescription(sb.ToString());
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
            }
            catch(Exception ex)
            {
                embed.UnknownError();
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                Console.WriteLine(ex.ToString());
            }
        }

        [SlashCommand("roadmap", "Returns a list of upcoming features.")]
        public async Task RoadmapCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new MidnightEmbedBuilder();
            embed.WithTitle("Roadmap");

            StringBuilder sb = new();

            sb.AppendLine("**Upcoming Features**");
            sb.AppendLine("**/bah »** Returns Bah info.");
            sb.AppendLine("**/lms »** Returns LMS info.");
            sb.AppendLine("**/koth »** Returns Koth info.");
            sb.AppendLine("**/suggest »** A command to suggest new features or improve existing ones.");
            sb.AppendLine("**/bug »** A command to report bugs.");
            sb.AppendLine();
            sb.AppendLine("**-**A feature to track island xp over time (days/months).");
            sb.AppendLine("**-**Timed island xp test (5m, 15n, etc.).");
            sb.AppendLine();
            sb.AppendLine("*These will be added without notice, so keep an eye out for them.*");

            embed.WithDescription(sb.ToString());
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("help", "Returns information that helps you start using the bot.")]
        public async Task HelpCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new();
            embed.WithTitle("Help");

            StringBuilder sb = new();
            sb.AppendLine();
            sb.AppendLine("To use this bot, you will use discord's slash command feature.");
            sb.AppendLine();
            sb.AppendLine("Many of the commands are the same as they would be in-game.");
            sb.AppendLine("**Example:** /a OR /alliance, /is, /bal, /ap");
            sb.AppendLine();
            sb.AppendLine("For a comprehensive list of all commands, type / and click on the bot's picture.");

            embed.WithDescription(sb.ToString());

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }
    }
}
