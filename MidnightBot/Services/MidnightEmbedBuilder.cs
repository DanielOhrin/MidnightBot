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

        public MidnightEmbedBuilder WithImageUrl(string url)
        {
            _builder.WithImageUrl(url);
            return this;
        }

        public MidnightEmbedBuilder AddField(string name, string value, bool inline = false)
        {
            _builder.AddField(name, value, inline);
            return this;
        }

        public MidnightEmbedBuilder ClearFields()
        {
            _builder.ClearFields();
            return this;
        }
        public MidnightEmbedBuilder WithFooter(string? text = null, string? iconUrl = null)
        {
            _builder.WithFooter(text, iconUrl);
            return this;
        }

        public MidnightEmbedBuilder WithTitle(string title)
        {
            _builder.WithTitle(title);
            return this;
        }

        public MidnightEmbedBuilder WithDescription(string description)
        {
            _builder.WithDescription(description);
            return this;
        }

        public MidnightEmbedBuilder WithPlayerHead(string playerName)
        {
            _builder.WithImageUrl($"https://minotar.net/helm/{playerName}/200.png");
            return this;
        }

        public DiscordEmbed Build()
        { 
            return _builder.Build();
        }

        public MidnightEmbedBuilder Error(string errorMessage)
        {
            _builder.ClearFields().WithImageUrl("").WithTitle("**Error**").WithDescription(errorMessage).WithColor(DiscordColor.DarkRed);
            return this;
        }

        public MidnightEmbedBuilder UnknownError()
        {
            _builder.ClearFields().WithImageUrl("").WithTitle("**AN ERROR OCCURED**").WithDescription("**An unknown error occured. Please contact a developer.**").WithColor(DiscordColor.DarkRed);
            return this;
        }
    }
}
