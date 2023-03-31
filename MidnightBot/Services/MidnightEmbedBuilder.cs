using DSharpPlus.Entities;

namespace MidnightBot.Services
{
    //! This class represents the wrapper for every Embed this bot sends.
    public class MidnightEmbedBuilder
    {
        private readonly DiscordEmbedBuilder _builder;
        private readonly DiscordColor _color = DiscordColor.Purple;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public IReadOnlyList<DiscordEmbedField> Fields => _builder.Fields;
        public MidnightEmbedBuilder()
        {
            _builder = new DiscordEmbedBuilder().WithAuthor("Midnight Sky", null, "https://cdn.discordapp.com/attachments/1056341873905635398/1085322008222519527/Midnight.jpg").WithColor(_color);
        }

        public DiscordEmbed WithImageUrl(string url)
        {
            return _builder.WithImageUrl(url);
        }

        public DiscordEmbed AddField(string name, string value, bool inline = false)
        {
            return _builder.AddField(name, value, inline);
        }

        public DiscordEmbed ClearFields()
        {
            return _builder.ClearFields();
        }
        public DiscordEmbed WithFooter(string? text = null, string? iconUrl = null)
        {
            return _builder.WithFooter(text, iconUrl);
        }

        public DiscordEmbed WithTitle(string title)
        {
            return _builder.WithTitle(title);
        }

        public DiscordEmbed WithDescription(string description)
        {
            return _builder.WithDescription(description);
        }

        public DiscordEmbed Build()
        {
            DiscordEmbedBuilder builder = _builder;

            if (Title != null)
            {
                builder = builder.WithTitle(Title);
            }

            if (Description != null)
            {
                builder = builder.WithDescription(Description);
            }

            return builder.Build();
        }
    }
}
