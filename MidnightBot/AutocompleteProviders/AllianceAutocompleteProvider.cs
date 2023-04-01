using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace MidnightBot.AutocompleteProviders
{
    internal class AllianceAutocompleteProvider : IAutocompleteProvider
    {
        public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            return new List<DiscordAutoCompleteChoice>
            {
                new DiscordAutoCompleteChoice("player", "player"),
                new DiscordAutoCompleteChoice("alliance", "alliance") 
            };
        }
    }
}
