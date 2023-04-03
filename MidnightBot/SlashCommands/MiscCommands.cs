using System.Text;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace MidnightBot.SlashCommands
{
    public class MiscCommands : ApplicationCommandModule
    {
        [SlashCommand("roadmap", "Returns a list of upcoming features.")]
        public async Task RoadmapCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().WithTitle("Roadmap").WithColor(DiscordColor.Purple).WithFooter("Server: midnightsky.net");

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

        [SlashCommand("help", "Helps you start using the bot.")]
        public async Task HelpCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().WithTitle("Help").WithColor(DiscordColor.Purple).WithFooter("Server: midnightsky.net");

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
