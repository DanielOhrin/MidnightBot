using System.Net.Http.Headers;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

using MidnightBot.Enums;
using MidnightBot.Data.Models;
using EnumOps;

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

        public static async Task<Player?> GetPlayerAsync(string playerName)
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/player?player_name={playerName}&key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<Player>(stream);
            }
        }

        public static async Task<PlayerWithEconomy?> GetBalanceAsync(string playerId, string economyType)
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/economy/balance?account_id={playerId}&economy={economyType}&key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<PlayerWithEconomy>(stream);
            }
        }

        public static async Task<TopTenPlayerWithEconomy?> GetTopAsync(string economyType)
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/economy/top?economy={economyType}&key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<TopTenPlayerWithEconomy>(stream);
            }
        }

        public static async Task<Alliance?> GetAllianceAsync(GetAllianceSearchTypeEnum searchType, string searchValue)
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/alliance?{searchType.GetDescription()}={searchValue}&key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<Alliance>(stream);
            }
        }

        public static async Task<Island?> GetIslandAsync(string islandId)
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/hourlyxp/single?island_id={islandId}&key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<Island>(stream);
            }
        }
        
        public static async Task<TopIslands?> GetTopIslandsAsync(long amount)
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/hourlyxp/top?top={amount}&key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<TopIslands>(stream);
            }
        }

        public static async Task<KingOfTheHill?> GetKingOfTheHillAsync()
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/koth?key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<KingOfTheHill>(stream);
            }
        }

        public static async Task<BlackAuctionHouse?> GetBlackAuctionHouseAsync()
        {
            using (var client = new MidnightClient().Client)
            {
                await using Stream stream = await client.GetStreamAsync($"{_url}/bah?key={_apiKey}");

                return await JsonSerializer.DeserializeAsync<BlackAuctionHouse>(stream);
            }
        }
    }
}