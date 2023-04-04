using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace MidnightBot.AutocompleteProviders
{
    internal class AdventureAutocompleteProvider : IAutocompleteProvider
    {
        public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            return new List<DiscordAutoCompleteChoice>
            {
                new DiscordAutoCompleteChoice("bandit", "bandit"),
                new DiscordAutoCompleteChoice("wasteland", "wasteland"),
                new DiscordAutoCompleteChoice("demonic", "demonic")
            };
        }
    }
}
