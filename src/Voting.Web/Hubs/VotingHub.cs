using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Voting.Web.Hubs
{
    public class VotingHub : Hub
    {
        private static VotingResults results = new VotingResults();
        private static object lockObject = new object();

        public async Task Vote(string vote)
        {
            lock (lockObject)
            {
                if (vote == "cats")
                {
                    results.Cats += 1;
                }
                else if (vote == "dogs")
                {
                    results.Dogs += 1;
                }
            }
            await Clients.All.SendAsync("resultsChanged", results);
        }

        public static async Task Reset(IHubContext<VotingHub> context)
        {
            lock (lockObject)
            {
                results.Dogs = results.Cats = 0;
            }
            await context.Clients.All.SendAsync("resultsChanged", results);
        }

        public async override Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("resultsChanged", results);
            await base.OnConnectedAsync();
        }
    }

    public class VotingResults
    {
        public int Cats { get; set; } = 0;
        public int Dogs { get; set; } = 0;
    }
}