using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace MidnightBot.AutocompleteProviders
{
    internal class TopAutocompleteProvider : IAutocompleteProvider
    {
        public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            return new List<DiscordAutoCompleteChoice>
            {
                new DiscordAutoCompleteChoice("balance", "balance"),
                new DiscordAutoCompleteChoice("ap", "ap"),
                new DiscordAutoCompleteChoice("fairy", "fairy"),
                new DiscordAutoCompleteChoice("fishing", "fishing"),
                new DiscordAutoCompleteChoice("dwarf", "dwarf")
            };
        }
    }
}
