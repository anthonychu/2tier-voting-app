using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Voting.Web.Hubs;

namespace Voting.Web.Controllers
{
    public class ResetController
    {
        private readonly IHubContext<VotingHub> context;
        private readonly VotingService votingService;

        public ResetController(IHubContext<VotingHub> context, VotingService votingService)
        {
            this.context = context;
            this.votingService = votingService;
        }

        [HttpPost("api/reset")]
        public async Task Post()
        {
            var results = await votingService.ResetResultsAsync();
            await VotingHub.ResetAsync(context, results);
        }
    }
}