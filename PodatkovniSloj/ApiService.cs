using System;
using System.Collections.Generic;
using PodatkovniSloj.Models;
using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PodatkovniSloj
{

    public class ApiService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<List<Data>> GetTeamsAsync(string championship)
        {
            var url = $"http://worldcup-vua.nullbit.hr/{championship}/teams/results";
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<Data>>(response);
            
        }
        public async Task<List<Match>> GetMatchesAsync(string championship)
        {
            var url = $"http://worldcup-vua.nullbit.hr/{championship}/matches";
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<Match>>(response);

        }
        /*public async Task<List<Data>> GetTeamsAsync(string championship)
        {
            var url = $"http://worldcup-vua.nullbit.hr/{championship}/teams/results";
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<Data>>(response);

        }*/
    }
}