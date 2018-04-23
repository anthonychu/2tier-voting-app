using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Voting.Web.Hubs
{
    public class VotingHub : Hub
    {
        private readonly VotingService votingService;

        public VotingHub(VotingService votingService)
        {
            this.votingService = votingService;
        }

        public async Task Vote(string vote)
        {
            var results = await votingService.VoteAsync(vote);
            await Clients.All.SendAsync("resultsChanged", results);
        }

        public static async Task ResetAsync(IHubContext<VotingHub> context, VotingResults results)
        {
            await context.Clients.All.SendAsync("resultsChanged", results);
        }

        public async override Task OnConnectedAsync()
        {
            var results = await votingService.GetResultsAsync();
            await Clients.Caller.SendAsync("resultsChanged", results);
            await base.OnConnectedAsync();
        }
    }
}