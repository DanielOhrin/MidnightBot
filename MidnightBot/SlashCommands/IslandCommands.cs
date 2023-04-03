using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using MidnightBot.Services;
using MidnightBot.Data.API;
using MidnightBot.Data.Models;
using System.Text;

namespace MidnightBot.SlashCommands
{
    [SlashCommandGroup("is", "Group of island commands")]
    public class IslandCommands : ApplicationCommandModule
    {
        [SlashCommand("xp", "Returns the amount of xp that the specified player's island has grinded so far in the current hour.")]
        public async Task IslandHourlyCommand(InteractionContext ctx, [Option("name", "IGN of the player who's island xp you would like to see.")] string playerName)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            Player? player = null;

            try
            {
                player = await MidnightAPI.GetPlayer(playerName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().WithColor(DiscordColor.Purple);

            if (player != null)
            {

                Island? island = await MidnightAPI.GetIsland(player.Id);

                if (island != null)
                {
                    embed.WithTitle($"{player.Name}'s Island").WithImageUrl($"https://minotar.net/helm/{playerName}/200.png");
                    embed.AddField($"Xp Gained", BotUtils.FormatNumber(island.Amount.ToString()));
                    embed.AddField($"Time", $"{DateTime.Now.Minute}m {DateTime.Now.Second}s");
                    embed.WithFooter("Server: play.midnightsky.net");
                }
                else
                {
                    embed.WithTitle("**AN ERROR OCCURED**").WithDescription("An unknown error has occured. Please contact a developer.").WithColor(DiscordColor.DarkRed);
                }
            }
            else
            {
                embed.WithTitle("**Error**").WithDescription($"Player **{playerName}** was not found.").WithColor(DiscordColor.DarkRed);
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("xptop", "Returns the top islands and their xp gain in the current hour.")]
        public async Task IslandHourlyTopCommand(InteractionContext ctx, [Minimum(1)][Maximum(50)][Option("amount", "Amount of islands you want to see.")] long amount)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            List<Island>? islands = (await MidnightAPI.GetTopIslands(amount))?.Islands;

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().WithColor(DiscordColor.Purple);

            if (islands != null && islands.Count == amount)
            {
                embed.WithTitle($"Top {amount} Hourly Island Xp");
                
                StringBuilder sb = new();
                sb.AppendLine($"**Time**");
                sb.AppendLine($"{DateTime.Now.Minute}m {DateTime.Now.Second}s");
                sb.AppendLine();

                try
                {
                    for (int i = 0; i < islands.Count; i++)
                    {
                        Island island = islands[i];

                        sb.AppendLine($"**{i + 1}. {island.Name} »** {BotUtils.FormatNumber(island.Amount.ToString())}");
                        embed.WithFooter("Server: play.midnightsky.net");
                    }
                    embed.WithDescription(sb.ToString());
                }
                catch (Exception)
                {
                    embed.WithTitle("**AN ERROR OCCURED**").WithDescription("An unknown error has occured. Please contact a developer.").WithColor(DiscordColor.DarkRed);
                }
            }
            else
            {
                embed.WithTitle("**AN ERROR OCCURED**").WithDescription("An unknown error has occured. Please contact a developer.").WithColor(DiscordColor.DarkRed);
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }
    }
}
