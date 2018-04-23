using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Voting.Web
{
    public class VotingService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = 
            Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5001";

        public VotingService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<VotingResults> VoteAsync(string vote)
        {
            var response = await httpClient.PostAsync($"{baseUrl}/api/votes?vote={WebUtility.UrlEncode(vote)}", null);
            var results = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VotingResults>(results);
        }

        public async Task<VotingResults> GetResultsAsync()
        {            
            var response = await httpClient.GetAsync($"{baseUrl}/api/votes");
            var results = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VotingResults>(results);
        }

        public async Task<VotingResults> ResetResultsAsync()
        {
            var response = await httpClient.DeleteAsync($"{baseUrl}/api/votes");
            var results = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VotingResults>(results);
        }
    }
}