using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace MidnightBot.AutocompleteProviders
{
    internal class RealmAutocompleteProvider : IAutocompleteProvider
    {
        public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            return new List<DiscordAutoCompleteChoice>
            {
                new DiscordAutoCompleteChoice("dwarf", "DWARF_POINTS"),
                new DiscordAutoCompleteChoice("fairy", "FAIRY_POINTS"),
                new DiscordAutoCompleteChoice("fishing", "FISHING_POINTS")
            };
        }
    }
}
