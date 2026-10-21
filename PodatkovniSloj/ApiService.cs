using System.Text.Json;
using PodatkovniSloj.Models;

namespace PodatkovniSloj
{
    public class ApiService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<List<TeamResult>> GetTeamsAsync(string championship)
        {
            var url = $"http://worldcup-vua.nullbit.hr/{championship}/teams/results";
            using var stream = await _httpClient.GetStreamAsync(url);
            return await JsonSerializer.DeserializeAsync<List<TeamResult>>(stream, WorldCupJson.Options) ?? [];
        }

        public async Task<List<Match>> GetMatchesAsync(string championship)
        {
            var url = $"http://worldcup-vua.nullbit.hr/{championship}/matches";
            using var stream = await _httpClient.GetStreamAsync(url);
            return await JsonSerializer.DeserializeAsync<List<Match>>(stream, WorldCupJson.Options) ?? [];
        }
    }
}
