namespace MidnightBot.Services
{
    public static class AdventureBossFields
    {
        private static readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> _data = new()
        {
            { "Bandit", new()
                {
                    { "Bosses", new()
                        {
                            {"**Placeholder**", "Description"},
                            {"**Placeholder2**", "Description"}
                        }
                    },
                    { "Minibosses", new()
                        {
                            { "Saj", "Spawns from killing **placeholder** mobs around CORDS HERE. Gear is _____"},
                            { "Bandit Assassin", "Spawns from killing **vicious** mobs around CORDS HERE. Gear is heavily focused around strength & crits."},
                            { "Beedus", "Spawns from killing **thieving** mobs around CORDS HERE. Gear is well-balanced overall."},
                            { "Lament", "Spawns from killing **placeholder** mobs around CORDS HERE. Gear is _____"},
                            { "Tree Protector", "Spawns from killing **tank** mobs around CORDS HERE. Gear is _____"}
                        }
                    }
                }
            },
            { "Wasteland", new()
                {
                    { "Bosses", new()
                        {
                            {"**Placeholder**", "Description"},
                            {"**Placeholder2**", "Description"}
                        }
                    },
                    { "Minibosses", new()
                        {
                            { "**Placeholder**", "Description"},
                            { "**Placeholder2**", "Description"}
                        }
                    }
                }
            },
            { "Demonic", new()
                {
                    { "Bosses", new()
                        {
                            {"**Placeholder**", "Description"},
                            {"**Placeholder2**", "Description"}
                        }
                    },
                    { "Minibosses", new()
                        {
                            { "**Placeholder**", "Description"},
                            { "**Placeholder2**", "Description"}
                        }
                    }
                }
            }
        };

        public static void Add(MidnightEmbedBuilder builder, string adventure, string type)
        {
            Dictionary<string, string> bossData = _data[adventure][type];

            foreach(KeyValuePair<string, string> kv in bossData)
            {
                builder.AddField(kv.Key, kv.Value);
            }
        }
    }
}
