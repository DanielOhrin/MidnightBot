using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using MidnightBot.Data.API;
using MidnightBot.Data.Models;
using MidnightBot.Services;

namespace MidnightBot.SlashCommands
{
    public class ApiCommands : ApplicationCommandModule
    {
        [SlashCommand("bal", "Returns the current balance of the specified player.")]
        public async Task BalCommand(InteractionContext ctx, [Option("player", "IGN of the player's balance you want to see.")] string playerName)
        {
            //! Create a "thinking..." response
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            Player? player = null;
            
            try
            {
                player = await MidnightAPI.GetPlayer(playerName);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().WithColor(DiscordColor.Purple);

            if (player != null)
            {
                PlayerWithEconomy? playerWithBal = await MidnightAPI.GetBalance(player.Id, "MONEY");

                if (playerWithBal != null)
                {
                    embed.WithTitle(playerWithBal.Name).WithImageUrl($"https://minotar.net/helm/{playerName}/200.png");
                    embed.AddField("Balance", "$" + BotUtils.FormatNumber(playerWithBal.Balance));
                    embed.AddField("Leaderboard Position", $"#{playerWithBal.Position}");
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
        [SlashCommand("ap", "Returns the current AP of the specified player.")]
        public async Task APCommand(InteractionContext ctx, [Option("player", "IGN of the player's balance you want to see.")] string playerName)
        {
            //! Create a "thinking..." response
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            Player? player = null;
            
            try
            {
                player = await MidnightAPI.GetPlayer(playerName);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().WithColor(DiscordColor.Purple);

            if (player != null)
            {
                PlayerWithEconomy? playerWithBal = await MidnightAPI.GetBalance(player.Id, "AP");

                if (playerWithBal != null)
                {
                    embed.WithTitle(playerWithBal.Name).WithImageUrl($"https://minotar.net/helm/{playerName}/200.png");
                    embed.AddField("AP", BotUtils.FormatNumber(playerWithBal.Balance.Substring(0, playerWithBal.Balance.IndexOf("."))));
                    embed.AddField("Leaderboard Position", $"#{playerWithBal.Position}");
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
    }
}
