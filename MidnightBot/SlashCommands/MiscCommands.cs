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

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().WithTitle("Roadmap").WithColor(DiscordColor.Purple);

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
    }
}
