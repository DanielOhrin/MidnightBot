﻿using System.Linq.Expressions;
using System.Text;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using MidnightBot.AutocompleteProviders;
using MidnightBot.Data.API;
using MidnightBot.Data.Models;
using MidnightBot.Enums;
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

            MidnightEmbedBuilder embed = new();
            Player? player = null;

            try
            {
                player = await MidnightAPI.GetPlayerAsync(playerName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }


            if (player?.Id != null)
            {
                PlayerWithEconomy? playerWithBal = await MidnightAPI.GetBalanceAsync(player.Id, "MONEY");

                if (playerWithBal != null)
                {
                    embed.WithPlayerHead(playerName).WithTitle(playerWithBal.Name);
                    embed.AddField("Balance", "$" + BotUtils.FormatNumber(playerWithBal.Balance));
                    embed.AddField("Leaderboard Position", $"#{playerWithBal.Position + 1}");
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
        [SlashCommand("ap", "Returns the current AP of the specified player.")]
        public async Task APCommand(InteractionContext ctx, [Option("player", "IGN of the player's AP you want to see.")] string playerName)
        {
            //! Create a "thinking..." response
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
                PlayerWithEconomy? playerWithBal = await MidnightAPI.GetBalanceAsync(player.Id, "AP");

                if (playerWithBal != null)
                {
                    embed.WithPlayerHead(playerName).WithTitle(playerWithBal.Name);
                    embed.AddField("AP", BotUtils.FormatNumber(playerWithBal.Balance.Substring(0, playerWithBal.Balance.IndexOf("."))));
                    embed.AddField("Leaderboard Position", $"#{playerWithBal.Position + 1}");
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

        [SlashCommand("realmpoints", "Returns the player's current points in each realm.")]
        public async Task RealmPointsCommand(InteractionContext ctx, [Option("player", "IGN of the player's realm points you want to see.")] string playerName)
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
                embed.WithPlayerHead(playerName).WithTitle(player.Name);

                List<string> pointTypes = new() { "DWARF", "FISHING", "FAIRY" };
                foreach (string pointType in pointTypes)
                {
                    try
                    {
                        PlayerWithEconomy? playerWithBal = await MidnightAPI.GetBalanceAsync(player.Id, pointType + "_POINTS");

                        if (playerWithBal != null)
                        {
                            embed.AddField(pointType[..1].ToUpper() + pointType[1..].ToLower() + " Realm", BotUtils.FormatNumber(playerWithBal.Balance.Substring(0, playerWithBal.Balance.IndexOf("."))) + $" (#{playerWithBal.Position + 1})");
                        }
                        else
                        {
                            embed.UnknownError();
                            break;
                        }
                    }
                    catch (HttpRequestException)
                    {
                        embed.AddField(pointType[..1].ToUpper() + pointType[1..].ToLower() + " Realm", "N/A");
                    }
                }
            }
            else
            {
                embed.Error($"Player **{playerName}** was not found.");
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("profile", "Returns all available information on the specified player.")]
        public async Task ProfileCommand(InteractionContext ctx, [Option("player", "IGN of the player's stats you want to see.")] string playerName)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new();
            Player? player = null;

            try
            {
                player = await MidnightAPI.GetPlayerAsync(playerName);
            }
            catch (HttpRequestException)
            {
                embed.Error($"Player **{playerName}** was not found.");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            PlayerWithEconomy? playerWithBal = null;
            try
            {
                //! Fetch and Add the balance
                playerWithBal = await MidnightAPI.GetBalanceAsync(player.Id, "MONEY");

                embed.WithPlayerHead(playerName).WithTitle(playerWithBal.Name);
                embed.AddField("Balance", "$" + $"{BotUtils.FormatNumber(playerWithBal.Balance)} (#{playerWithBal.Position + 1})");

                //! Fetch and Add the AP
                playerWithBal = await MidnightAPI.GetBalanceAsync(player.Id, "AP");

                embed.AddField("AP", BotUtils.FormatNumber(playerWithBal.Balance.Substring(0, playerWithBal.Balance.IndexOf("."))) + $" (#{playerWithBal.Position + 1})");
            }
            catch (HttpRequestException)
            {
                embed.UnknownError();
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            //! Fetch and Add the realm points
            List<string> pointTypes = new() { "DWARF", "FISHING", "FAIRY" };
            foreach (string pointType in pointTypes)
            {
                try
                {
                    playerWithBal = await MidnightAPI.GetBalanceAsync(player.Id, pointType + "_POINTS");

                    if (playerWithBal != null)
                    {
                        embed.AddField(pointType[..1].ToUpper() + pointType[1..].ToLower() + " Realm", BotUtils.FormatNumber(playerWithBal.Balance.Substring(0, playerWithBal.Balance.IndexOf("."))) + $" (#{playerWithBal.Position + 1})");
                    }
                    else
                    {
                        embed.UnknownError();
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                        return;
                    }
                }
                catch (HttpRequestException)
                {
                    embed.AddField(pointType[..1].ToUpper() + pointType[1..].ToLower() + " Realm", "N/A");
                }
            }

            Alliance? alliance = await MidnightAPI.GetAllianceAsync(GetAllianceSearchTypeEnum.PlayerName, player.Name);

            if (alliance?.Id != null)
            {
                embed.AddField("Alliance", $"{alliance.Name} ({alliance.Members.First(x => x.Name == player.Name).Role})");
            }
            else
            {
                embed.AddField("Alliance", "N/A");
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("top", "Returns the top 10 players in the specified category")]
        public async Task TopCommand(InteractionContext ctx, [Option("category", "The leaderboard category to display", true)][Autocomplete(typeof(TopAutocompleteProvider))] string category)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new();

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
                    embed.Error("Invalid category given.");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                    return;
            }

            List<PlayerWithEconomy>? players = null;

            try
            {
                players = (await MidnightAPI.GetTopAsync(category))?.Players;
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
                embed.Error("There are no players in this leaderboard yet.");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            StringBuilder sb = new();

            for (int i = 0; i < 10; i++)
            {
                sb.AppendLine($"**{i + 1}. {players[i].Name}** » {(category == "MONEY" ? "$" : null)}{BotUtils.FormatNumber(category == "MONEY" ? players[i].Balance : players[i].Balance.Substring(0, players[i].Balance.IndexOf(".")))}");
            }

            embed.WithDescription(sb.ToString());

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

    }
}
