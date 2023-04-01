using DSharpPlus.SlashCommands;

using MidnightBot.AutocompleteProviders;

namespace MidnightBot.SlashCommands
{
    [SlashCommandGroup("alliance", "Alliance commands")]
    public class AllianceCommands : ApplicationCommandModule
    {
        [SlashCommand("info", "Returns basic information about an alliance.")]
        public async Task AllianceInfoCommand(InteractionContext ctx, [Option("name", "Name of a player or alliance.")] string searchValue)
        {

        }

        [SlashCommand("who", "Returns basic information about an alliance.")]
        public async Task AllianceWhoCommand(InteractionContext ctx, [Option("name", "Name of a player or alliance.")] string searchValue)
        {

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
