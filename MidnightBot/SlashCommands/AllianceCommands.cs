using System.Diagnostics.CodeAnalysis;
using System.Text;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using MidnightBot.AutocompleteProviders;
using MidnightBot.Data.API;
using MidnightBot.Data.Models;

namespace MidnightBot.SlashCommands
{
    [SlashCommandGroup("alliance", "Alliance commands")]
    public class AllianceCommands : ApplicationCommandModule
    {
        [SlashCommand("info", "Returns basic information about an alliance.")]
        public async Task AllianceInfoCommand(InteractionContext ctx, [Option("name", "Name of a player or alliance.")] string searchValue)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().WithColor(DiscordColor.Purple);

            Alliance? alliance = await MidnightAPI.GetAlliance("player", searchValue);

            if (alliance?.Id == null)
            {
                alliance = await MidnightAPI.GetAlliance("alliance", searchValue);
            }

            if (alliance?.Id == null)
            {
                embed.WithTitle("**Error**").WithDescription("There is no player or alliance that matches that name.").WithColor(DiscordColor.DarkRed);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            embed.WithTitle(alliance.Name);
            
            StringBuilder sb = new($"**Leader »** {alliance.Members.First(x => x.Role.ToUpper() == "LEADER" ).Name}");

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
                        enemies.Add((await MidnightAPI.GetAlliance("alliance_id", enemy.Key))?.Name);
                    }

                    if (enemies.Contains(null))
                    {
                        throw new NullReferenceException();
                    }

                    sb.Append(string.Join(",  ", enemies.Order()));
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message + "\n" + ex.StackTrace);
                    embed.WithTitle("**UNKNOWN ERROR**").WithDescription("An unknown error occured. Please contact a developer.").WithColor(DiscordColor.DarkRed);
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

        }

        [SlashCommand("ap", "Returns a list of the alliance's members, their rankings, and the alliance's total ap.")]
        public async Task AllianceAPCommand(InteractionContext ctx, [Option("name", "Name of a player or alliance.")] string searchValue)
        {

        }

        [SlashCommand("realmpoints", "Returns a list of the alliance's members, their rankings, and the alliance's total points.")]
        public async Task AllianceRealmPointsCommand(InteractionContext ctx, [Option("name", "Name of a player or alliance.")] string searchValue, [Option("realm", "Which realm's points to show.", true)][Autocomplete(typeof(RealmAutocompleteProvider))] string realmType)
        {

        }
    }
}
