using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using MidnightBot.Data.API;
using MidnightBot.Data.BotAPI.Models;
using MidnightBot.Data.MidnightAPI.Models;
using MidnightBot.Services;

namespace MidnightBot.SlashCommands
{
    [SlashCommandGroup("api", "A group of commands to edit your API key.")]
    public class APICommands : ApplicationCommandModule
    {
        [SlashCommand("set", "Registers your account with the given API key.")]
        public async Task APIAddCommand(InteractionContext ctx, [Option("key", "Your personal key from typing /api on the server.")] string apiKey)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MidnightEmbedBuilder embed = new();

            Key? key = null;
            Console.WriteLine("hi");
            try
            {
                key = await MidnightAPI.GetKeyAsync(apiKey);
            }
            catch (HttpRequestException)
            {
                embed.Error("Invalid API key.");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                return;
            }
            Console.WriteLine("hi");
            try
            {
                var db = new MidnightSkyBotContext();

                UserProfile? profile = db.UserProfiles.Where(x => x.Id == (long)ctx.User.Id).FirstOrDefault();

                if (profile != null)
                {
                    if (profile.ApiKey != apiKey)
                    {
                        embed.Error("Replacing API keys is not yet implemented.");
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                        return;
                    }

                    embed.Error("You are already using that key.");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                    return;
                }
                else
                {
                    Console.WriteLine("hi");
                    bool isDuplicate = db.UserProfiles.Where(x => x.ApiKey == apiKey).FirstOrDefault() != null;

                    if (isDuplicate)
                    {
                        embed.Error("Another account is using that key. If this is unexpected, contact a developer.");
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                        return;
                    }

                    profile = new() { Id = (long)ctx.User.Id, ApiKey = apiKey, PlayerId = key?.PlayerId };
                    db.Add(profile);
                    db.SaveChanges();

                    embed.WithTitle("Success!").WithDescription($"\nYou may now use certain commands without providing a player name, and track your island xp in-depth!").WithColor(DiscordColor.Green);
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
                }
            }
            catch (Exception)
            {
                embed.UnknownError();
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
            }
        }


    }
}
