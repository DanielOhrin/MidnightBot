using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace MidnightBot.AutocompleteProviders
{
    internal class BossTypeAutocompleteProvider : IAutocompleteProvider
    {
        public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            return new List<DiscordAutoCompleteChoice>
            {
                new DiscordAutoCompleteChoice("miniboss", "miniboss"),
                new DiscordAutoCompleteChoice("boss", "boss")
            };
        }
    }
}
