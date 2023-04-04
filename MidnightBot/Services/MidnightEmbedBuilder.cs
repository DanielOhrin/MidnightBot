using DSharpPlus.Entities;

namespace MidnightBot.Services
{
    //! This class represents the wrapper for every Embed this bot sends.
    public class MidnightEmbedBuilder
    {
        private readonly DiscordEmbedBuilder _builder;
        private readonly DiscordColor _defaultColor = DiscordColor.Purple;
        public MidnightEmbedBuilder()
        {
            _builder = new DiscordEmbedBuilder().WithColor(_defaultColor).WithFooter("Server: midnightsky.net");
        }

        public DiscordEmbedBuilder WithImageUrl(string url)
        {
            return _builder.WithImageUrl(url);
        }

        public DiscordEmbedBuilder AddField(string name, string value, bool inline = false)
        {
            return _builder.AddField(name, value, inline);
        }

        public DiscordEmbedBuilder ClearFields()
        {
            return _builder.ClearFields();
        }
        public DiscordEmbedBuilder WithFooter(string? text = null, string? iconUrl = null)
        {
            return _builder.WithFooter(text, iconUrl);
        }

        public DiscordEmbedBuilder WithTitle(string title)
        {
            return _builder.WithTitle(title);
        }

        public DiscordEmbedBuilder WithDescription(string description)
        {
            return _builder.WithDescription(description);
        }

        public DiscordEmbed Build()
        {
            DiscordEmbedBuilder builder = _builder;

            return builder.Build();
        }

        public DiscordEmbedBuilder Error(string errorMessage)
        {
            return _builder.ClearFields().WithImageUrl("").WithTitle("**Error**").WithDescription(errorMessage).WithColor(DiscordColor.DarkRed);
        }

        public DiscordEmbedBuilder UnknownError()
        {
            return _builder.ClearFields().WithImageUrl("").WithTitle("**AN ERROR OCCURED**").WithDescription("**An unknown error occured. Please contact a developer.");
        }
    }
}
