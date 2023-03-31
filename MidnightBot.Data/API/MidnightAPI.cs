﻿using System.Net.Http.Headers;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

using MidnightBot.Data.Models;

namespace MidnightBot.Data.API
{
    internal class MidnightClient
    {
        public readonly HttpClient Client = default!;
        internal MidnightClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }

    public static class MidnightAPI
    {
        private static readonly string _url = "https://api.midnightsky.net";
        private static readonly string _apiKey = new ConfigurationBuilder().AddUserSecrets("bbb4920a-bccf-46bb-bdb0-0611680075ba").Build()["apiKey"];

        public static async Task<Player?> GetPlayer(string playerName)
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/player?player_name={playerName}&key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<Player>(stream);
            }
        }

        public static async Task<PlayerWithEconomy?> GetBalance(string playerId, string economyType)
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/economy/balance?account_id={playerId}&economy={economyType}&key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<PlayerWithEconomy>(stream);
            }
        }

        public static async Task<TopTenPlayerWithEconomy?> GetTop(string economyType)
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/economy/top?economy={economyType}&key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<TopTenPlayerWithEconomy>(stream);
            }
        }
    }
}