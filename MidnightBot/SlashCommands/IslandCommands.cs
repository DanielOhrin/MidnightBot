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
        [SlashCommand("xp", "Returns the amount of xp that the specified player's island has grinded in the current hour.")]
        public async Task IslandHourlyCommand(InteractionContext ctx, [Option("name", "IGN of the player who's island xp you would like to see.")] string playerName)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            Player? player = null;

            try
            {
                player = await MidnightAPI.GetPlayerAsync(playerName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }

            MidnightEmbedBuilder embed = new();

            if (player != null)
            {

                Island? island = await MidnightAPI.GetIslandAsync(player.Id);

                if (island != null)
                {
                    embed.WithTitle($"{player.Name}'s Island").WithImageUrl($"https://minotar.net/helm/{playerName}/200.png");
                    embed.AddField($"Xp Gained", BotUtils.FormatNumber(island.Amount.ToString()));
                    embed.AddField($"Time", $"{DateTime.Now.Minute}m {DateTime.Now.Second}s");
                }
                else
                {
                    embed.UnknownError();
                }
            }
            else
            {
                embed.Error($"Player **{playerName}** was not found.");
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("xptop", "Returns the top islands and their xp gain in the current hour.")]
        public async Task IslandHourlyTopCommand(InteractionContext ctx, [Minimum(1)][Maximum(50)][Option("amount", "Amount of islands you want to see.")] long amount)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            List<Island>? islands = (await MidnightAPI.GetTopIslandsAsync(amount))?.Islands;

            MidnightEmbedBuilder embed = new();

            if (islands != null)
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
                    }
                    if (islands.Count != amount)
                    {
                        sb.AppendLine();
                        sb.AppendLine("*All islands that have gained xp this hour have been displayed.*");
                    }

                    embed.WithDescription(sb.ToString());
                }
                catch (Exception)
                {
                    embed.UnknownError();
                }
            }
            else
            {
                embed.UnknownError();
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }
    }
}
