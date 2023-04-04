using System.Diagnostics.CodeAnalysis;
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
    [SlashCommandGroup("alliance", "Alliance commands")]
    public class AllianceCommands : ApplicationCommandModule
    {
        [SlashCommand("info", "Returns basic information about an alliance.")]
        public async Task AllianceInfoCommand(InteractionContext ctx, [Option("name", "Name of a player or alliance.")] string searchValue)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new();

            Alliance? alliance = await MidnightAPI.GetAlliance(GetAllianceSearchTypeEnum.PlayerName, searchValue);

            if (alliance?.Id == null)
            {
                alliance = await MidnightAPI.GetAlliance(GetAllianceSearchTypeEnum.AllianceName, searchValue);
            }

            if (alliance?.Id == null)
            {
                embed.Error("There is no player or alliance that matches that name.");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            embed.WithTitle(alliance.Name);

            StringBuilder sb = new($"**Leader »** {alliance.Members.First(x => x.Role.ToUpper() == "LEADER").Name}");

            List<string> admins = alliance.Members.Where(x => x.Role.ToUpper() == "ADMIN").OrderBy(x => x.Name).Select(x => x.Name).ToList();
            List<string> officers = alliance.Members.Where(x => x.Role.ToUpper() == "OFFICER").OrderBy(x => x.Name).Select(x => x.Name).ToList();
            List<string> members = alliance.Members.Where(x => x.Role.ToUpper() == "MEMBER").OrderBy(x => x.Name).Select(x => x.Name).ToList();


            sb.AppendLine();
            sb.Append("**Admins »** ");
            sb.Append(admins.Count == 0 ? "N/A" : string.Join(",  ", admins));

            sb.AppendLine();
            sb.Append("**Officers »** ");
            sb.Append(officers.Count == 0 ? "N/A" : string.Join(",  ", officers));

            sb.AppendLine();
            sb.AppendLine();
            sb.Append("**Members »** ");
            sb.Append(members.Count == 0 ? "N/A" : string.Join(",  ", members));

            sb.AppendLine();
            sb.AppendLine();
            sb.Append("**Enemies »** ");

            List<string?> enemies = new();
            if (alliance.Relations.Count > 0)
            {
                try
                {
                    foreach (KeyValuePair<string, string> enemy in alliance.Relations)
                    {
                        enemies.Add((await MidnightAPI.GetAlliance(GetAllianceSearchTypeEnum.AllianceId, enemy.Key))?.Name);
                    }

                    if (enemies.Contains(null))
                    {
                        throw new NullReferenceException();
                    }

                    sb.Append(string.Join(",  ", enemies.Order()));
                }
                catch (Exception)
                {
                    embed.UnknownError();
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                    return;
                }
            }
            else
            {
                sb.Append("N/A");
            }


            embed.WithDescription(sb.ToString());

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("who", "Returns basic information about an alliance.")]
        public async Task AllianceWhoCommand(InteractionContext ctx, [Option("name", "Name of a player or alliance.")] string searchValue)
        {
            await AllianceInfoCommand(ctx, searchValue);
        }

        [SlashCommand("bal", "Returns a list of the alliance's members, their rankings, and the alliance's total balance.")]
        public async Task AllianceBalCommand(InteractionContext ctx, [Option("name", "Name of a player or alliance.")] string searchValue)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new();

            Alliance? alliance = await MidnightAPI.GetAlliance(GetAllianceSearchTypeEnum.PlayerName, searchValue);

            if (alliance?.Id == null)
            {
                alliance = await MidnightAPI.GetAlliance(GetAllianceSearchTypeEnum.AllianceName, searchValue);
            }

            if (alliance?.Id == null)
            {
                embed.Error("There is no player or alliance that matches that name.");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            embed.WithTitle(alliance.Name);

            List<PlayerWithEconomy?> players = new();

            foreach (PlayerWithAlliance player in alliance.Members)
            {
                PlayerWithEconomy? newPlayer = await MidnightAPI.GetBalance(player.Id, "MONEY");
                if (newPlayer.Position != (-1))
                {
                    players.Add(newPlayer);
                }
                else
                {
                    players.Add(new PlayerWithEconomy(newPlayer.Id, newPlayer.Name, 99999, newPlayer.Balance));
                }
            }

            if (players.Contains(null))
            {
                embed.UnknownError();
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            string sum = "0.00";
            foreach (PlayerWithEconomy player in players)
            {
                sum = BotUtils.AddNumbers(sum, player.Balance);
            }

            StringBuilder sb = new("**Total Balance »** $" + BotUtils.FormatNumber(sum));

            sb.AppendLine();
            sb.AppendLine();

            players = players.OrderBy(x => x.Position).ToList();
            for (int i = 0; i < players.Count; i++)
            {
                sb.AppendLine($"**{i + 1}. {players[i].Name} »** ${BotUtils.FormatNumber(players[i].Balance)} {(players[i].Position != 99999 ? $"(#{players[i].Position + 1})" : "")}");
            }

            embed.WithDescription(sb.ToString());
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("ap", "Returns a list of the alliance's members, their rankings, and the alliance's total ap.")]
        public async Task AllianceAPCommand(InteractionContext ctx, [Option("name", "Name of a player or alliance.")] string searchValue)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new();

            Alliance? alliance = await MidnightAPI.GetAlliance(GetAllianceSearchTypeEnum.PlayerName, searchValue);

            if (alliance?.Id == null)
            {
                alliance = await MidnightAPI.GetAlliance(GetAllianceSearchTypeEnum.AllianceName, searchValue);
            }

            if (alliance?.Id == null)
            {
                embed.Error("There is no player or alliance that matches that name.");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            embed.WithTitle(alliance.Name);

            List<PlayerWithEconomy?> players = new();

            foreach (PlayerWithAlliance player in alliance.Members)
            {
                PlayerWithEconomy? newPlayer = await MidnightAPI.GetBalance(player.Id, "AP");
                if (newPlayer.Position != (-1))
                {
                    players.Add(newPlayer);
                }
                else
                {
                    players.Add(new PlayerWithEconomy(newPlayer.Id, newPlayer.Name, 99999, newPlayer.Balance));
                }
            }

            if (players.Contains(null))
            {
                embed.UnknownError();
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            string sum = "0.00";
            foreach (PlayerWithEconomy player in players)
            {
                if (player.Balance != "0.00")
                {
                    sum = BotUtils.AddNumbers(sum, player.Balance);
                }
            }

            sum = BotUtils.FormatNumber(sum);
            StringBuilder sb = new("**Total AP »** " + sum.Substring(0, sum.IndexOf(".")));

            sb.AppendLine();
            sb.AppendLine();

            players = players.OrderBy(x => x.Position).ToList();
            for (int i = 0; i < players.Count; i++)
            {
                string balance = BotUtils.FormatNumber(players[i].Balance);
                sb.AppendLine($"**{i + 1}. {players[i].Name} »** {balance.Substring(0, balance.IndexOf("."))} {(players[i].Position != 99999 ? $"(#{players[i].Position + 1})" : "")}");
            }

            embed.WithDescription(sb.ToString());
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("realmpoints", "Returns a list of the alliance's members, their rankings, and the alliance's total points.")]
        public async Task AllianceRealmPointsCommand(InteractionContext ctx, [Option("name", "Name of a player or alliance.")] string searchValue, [Option("realm", "Which realm's points to show.", true)][Autocomplete(typeof(RealmAutocompleteProvider))] string realmType)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new();

            Alliance? alliance = await MidnightAPI.GetAlliance(GetAllianceSearchTypeEnum.PlayerName, searchValue);

            if (alliance?.Id == null)
            {
                alliance = await MidnightAPI.GetAlliance(GetAllianceSearchTypeEnum.AllianceName, searchValue);
            }

            if (alliance?.Id == null)
            {
                embed.Error("There is no player or alliance that matches that name.");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            embed.WithTitle(alliance.Name);

            List<PlayerWithEconomy?> players = new();

            foreach (PlayerWithAlliance player in alliance.Members)
            {
                try
                {
                    PlayerWithEconomy? newPlayer = await MidnightAPI.GetBalance(player.Id, realmType);
                    if (newPlayer.Position != (-1))
                    {
                        players.Add(newPlayer);
                    }
                    else
                    {
                        players.Add(new PlayerWithEconomy(newPlayer.Id, newPlayer.Name, 99999, newPlayer.Balance));
                    }
                }
                catch (HttpRequestException)
                {
                    players.Add(new PlayerWithEconomy(player.Id, player.Name, 100000, "N/A"));
                }
            }

            if (players.Contains(null))
            {
                embed.UnknownError();
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            string sum = "0.00";
            foreach (PlayerWithEconomy player in players)
            {
                if (player.Balance != "N/A" && player.Balance != "0.00")
                {
                    sum = BotUtils.AddNumbers(sum, player.Balance);
                }
            }

            sum = BotUtils.FormatNumber(sum);
            StringBuilder sb = new($"**Total {(realmType[..1] + realmType[1..].ToLower()).Replace("_", " ")} »** " + sum.Substring(0, sum.IndexOf(".")));

            sb.AppendLine();
            sb.AppendLine();

            players = players.OrderBy(x => x.Position).ToList();
            for (int i = 0; i < players.Count; i++)
            {
                string balance = BotUtils.FormatNumber(players[i].Balance);
                sb.AppendLine($"**{i + 1}. {players[i].Name} »** {(balance != "N/A" ? balance.Substring(0, balance.IndexOf(".")) : balance)} {(players[i].Position != 100000 && players[i].Position != 99999 ? $"(#{players[i].Position + 1})" : "")}");
            }

            embed.WithDescription(sb.ToString());
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }
    }
}
