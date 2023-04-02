using System.Text;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using MidnightBot.AutocompleteProviders;
using MidnightBot.Data.API;
using MidnightBot.Data.Models;
using MidnightBot.Services;

namespace MidnightBot.SlashCommands
{
    public class EconomyCommands : ApplicationCommandModule
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
            }
            catch (Exception ex)
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
                    embed.AddField("Leaderboard Position", $"#{playerWithBal.Position + 1}");
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
        [SlashCommand("ap", "Returns the current AP of the specified player.")]
        public async Task APCommand(InteractionContext ctx, [Option("player", "IGN of the player's AP you want to see.")] string playerName)
        {
            //! Create a "thinking..." response
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
                PlayerWithEconomy? playerWithBal = await MidnightAPI.GetBalance(player.Id, "AP");

                if (playerWithBal != null)
                {
                    embed.WithTitle(playerWithBal.Name).WithImageUrl($"https://minotar.net/helm/{playerName}/200.png");
                    embed.AddField("AP", BotUtils.FormatNumber(playerWithBal.Balance.Substring(0, playerWithBal.Balance.IndexOf("."))));
                    embed.AddField("Leaderboard Position", $"#{playerWithBal.Position + 1}");
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

        [SlashCommand("realmpoints", "Similar to /ap and /bal, this returns the player's points and position in each realm.")]
        public async Task RealmPointsCommand(InteractionContext ctx, [Option("player", "IGN of the player's realm points you want to see.")] string playerName)
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
                embed.WithTitle(player.Name).WithImageUrl($"https://minotar.net/helm/{playerName}/200.png");

                List<string> pointTypes = new() { "DWARF", "FISHING", "FAIRY" };
                foreach (string pointType in pointTypes)
                {
                    try
                    {
                        PlayerWithEconomy? playerWithBal = await MidnightAPI.GetBalance(player.Id, pointType + "_POINTS");

                        if (playerWithBal != null)
                        {
                            embed.AddField(pointType[..1].ToUpper() + pointType[1..].ToLower() + " Realm", BotUtils.FormatNumber(playerWithBal.Balance.Substring(0, playerWithBal.Balance.IndexOf("."))) + $" (#{playerWithBal.Position + 1})");
                        }
                        else
                        {
                            embed = new DiscordEmbedBuilder().WithTitle("**AN ERROR OCCURED**").WithDescription("An unknown error has occured. Please contact a developer.").WithColor(DiscordColor.DarkRed);
                            break;
                        }
                    }
                    catch (HttpRequestException)
                    {
                        embed.AddField(pointType[..1].ToUpper() + pointType[1..].ToLower() + " Realm", "N/A");
                    }
                }
                embed.WithFooter("Server: play.midnightsky.net");
            }
            else
            {
                embed.WithTitle("**Error**").WithDescription($"Player **{playerName}** was not found.").WithColor(DiscordColor.DarkRed);
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("profile", "Shows all available information on the specified player.")]
        public async Task ProfileCommand(InteractionContext ctx, [Option("player", "IGN of the player's stats you want to see.")] string playerName)
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
                PlayerWithEconomy? playerWithBal = await MidnightAPI.GetBalance(player.Id, "MONEY");

                if (playerWithBal != null)
                {
                    embed.WithTitle(playerWithBal.Name).WithImageUrl($"https://minotar.net/helm/{playerName}/200.png");
                    embed.AddField("Balance", "$" + $"{BotUtils.FormatNumber(playerWithBal.Balance)} (#{playerWithBal.Position + 1})");
                }
                else
                {
                    embed.WithTitle("**AN ERROR OCCURED**").WithDescription("An unknown error has occured. Please contact a developer.").WithColor(DiscordColor.DarkRed);
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                    return;
                }


                playerWithBal = await MidnightAPI.GetBalance(player.Id, "AP");

                if (playerWithBal != null)
                {
                    embed.AddField("AP", BotUtils.FormatNumber(playerWithBal.Balance.Substring(0, playerWithBal.Balance.IndexOf("."))) + $" (#{playerWithBal.Position + 1})");
                }
                else
                {
                    embed.WithTitle("**AN ERROR OCCURED**").WithDescription("An unknown error has occured. Please contact a developer.").WithColor(DiscordColor.DarkRed);
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                    return;
                }

                List<string> pointTypes = new() { "DWARF", "FISHING", "FAIRY" };
                foreach (string pointType in pointTypes)
                {
                    try
                    {
                        playerWithBal = await MidnightAPI.GetBalance(player.Id, pointType + "_POINTS");

                        if (playerWithBal != null)
                        {
                            embed.AddField(pointType[..1].ToUpper() + pointType[1..].ToLower() + " Realm", BotUtils.FormatNumber(playerWithBal.Balance.Substring(0, playerWithBal.Balance.IndexOf("."))) + $" (#{playerWithBal.Position + 1})");
                        }
                        else
                        {
                            embed = new DiscordEmbedBuilder().WithTitle("**AN ERROR OCCURED**").WithDescription("An unknown error has occured. Please contact a developer.").WithColor(DiscordColor.DarkRed);
                            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                            return;
                        }
                    }
                    catch (HttpRequestException)
                    {
                        embed.AddField(pointType[..1].ToUpper() + pointType[1..].ToLower() + " Realm", "N/A");
                    }
                }

                Alliance? alliance = await MidnightAPI.GetAlliance("player", player.Name);

                if (alliance?.Id != null)
                {
                    embed.AddField("Alliance", $"{alliance.Name} ({alliance.Members.First(x => x.Name == player.Name).Role})");
                }
                else
                {
                    embed.AddField("Alliance", "N/A");
                }

                embed.WithFooter("Server: play.midnightsky.net");
            }
            else
            {
                embed.WithTitle("**Error**").WithDescription($"Player **{playerName}** was not found.").WithColor(DiscordColor.DarkRed);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("top", "Shows the top 10 players in the specified category")]
        public async Task TopCommand(InteractionContext ctx, [Option("category", "The leaderboard category to display", true)][Autocomplete(typeof(TopAutocompleteProvider))] string category)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().WithColor(DiscordColor.Purple);

            switch (category.ToLower())
            {
                case "ap":
                    category = "AP";
                    break;
                case "balance":
                    category = "MONEY";
                    break;
                case "fairy":
                    category = "FAIRY_POINTS";
                    break;
                case "fishing":
                    category = "FISHING_POINTS";
                    break;
                case "dwarf":
                    category = "DWARF_POINTS";
                    break;
                default:
                    embed = new DiscordEmbedBuilder().WithTitle("**Error**").WithDescription("Invalid category given.").WithColor(DiscordColor.DarkRed);
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                    return;
            }

            List<PlayerWithEconomy>? players = null;

            try
            {
                players = (await MidnightAPI.GetTop(category))?.Players;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }

            if (category.Contains("_"))
            {
                embed.WithTitle($"{(category[..1] + category[1..].ToLower()).Replace("_", " ")} Leaderboard");
            }
            else
            {
                embed.WithTitle($"{(category == "AP" ? "AP" : category[..1] + category[1..].ToLower())} Leaderboard");
            }

            if (players == null)
            {
                embed.WithTitle("**Error**").WithDescription("There are no players in this leaderboard yet.").WithColor(DiscordColor.DarkRed);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            StringBuilder sb = new();

            for (int i = 0; i < 10; i++)
            {
                sb.AppendLine($"**{i + 1}. {players[i].Name}** » {(category == "MONEY" ? "$" : null)}{BotUtils.FormatNumber(category == "MONEY" ? players[i].Balance : players[i].Balance.Substring(0, players[i].Balance.IndexOf(".")))}");
            }

            embed.WithDescription(sb.ToString()).WithFooter("Server: midnightsky.net");

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

    }
}
