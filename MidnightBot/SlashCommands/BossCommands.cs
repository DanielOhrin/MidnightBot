using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using MidnightBot.AutocompleteProviders;
using MidnightBot.Enums;
using MidnightBot.Services;

namespace MidnightBot.SlashCommands
{
    public class BossCommands : ApplicationCommandModule
    {
        [SlashCommand("bosslist", "Returns a list of bosses for the specified adventure")]
        public static async Task BossListCommand(InteractionContext ctx, [Option("adventure", "Which adventure?", true)][Autocomplete(typeof(AdventureAutocompleteProvider))] string adventure, [Option("type", "What type of boss?", true)][Autocomplete(typeof(BossTypeAutocompleteProvider))] string bossType)
        {
            //! Create a deferred response (thinking...)
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            //! Instantiate the embedbuilder
            MidnightEmbedBuilder embed = new();

            if (!Enum.GetNames(typeof(AdventureNamesEnum)).Any(x => x.Equals(adventure, StringComparison.CurrentCultureIgnoreCase)))
            {
                embed.Error("Invalid adventure type.");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            if (!Enum.GetNames(typeof(BossTypesEnum)).Any(x => x.Equals(bossType, StringComparison.CurrentCultureIgnoreCase)))
            {
                embed.Error("Invalid boss type.");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }

            //! Format the arguments to look nice for the embed's title
            adventure = adventure[..1].ToUpper() + adventure[1..].ToLower();
            bossType = bossType[..1].ToUpper() + bossType[1..].ToLower() + "es";
            embed.WithTitle($"{adventure} Adventure {bossType}");
            
            //! Add the fields 
            AdventureBossFields.Add(embed, adventure, bossType);

            //! Edit the "thinking..." message
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
        }
    }
}
