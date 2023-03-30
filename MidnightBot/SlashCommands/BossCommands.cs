using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using MidnightBot.Enums;
using MidnightBot.Services;

namespace MidnightBot.SlashCommands
{
    public class BossCommands : ApplicationCommandModule
    {
        [SlashCommand("bosslist", "Returns a list of bosses for the specified adventure")]
        public static async Task BossListCommand(InteractionContext ctx, [Option("adventure", "bandit, wasteland, demonic")] string adventure, [Option("type", "miniboss, boss")] string type)
        {
            //! Create a deferred response (thinking...)
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            //! Instantiate the embedbuilder
            MidnightEmbedBuilder embed = new();

            if (!Enum.GetNames(typeof(AdventureNamesEnum)).Any(x => x.Equals(adventure, StringComparison.CurrentCultureIgnoreCase)))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Invalid adventure type."));
                return;
            }

            if (!Enum.GetNames(typeof(BossTypesEnum)).Any(x => x.Equals(type, StringComparison.CurrentCultureIgnoreCase)))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Invalid boss type."));
                return;
            }

            //! Format the arguments to look nice for the embed's title
            adventure = adventure[..1].ToUpper() + adventure[1..].ToLower();
            type = type[..1].ToUpper() + type[1..].ToLower() + "es";
            embed.Title = $"{adventure} Adventure {type}";
            
            try
            {
                AdventureBossFields.Add(embed, adventure, type);
            }
            catch (Exception e)
            {
                embed.Description = e.Message + "\n" + e.Source + "\n" + e.StackTrace + "\n" + e.InnerException;
            }
            //! Add the fields 

            //! Edit the "thinking..." message
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }
    }
}
