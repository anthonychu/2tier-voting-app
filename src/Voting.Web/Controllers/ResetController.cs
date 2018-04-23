using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Voting.Web.Hubs;

namespace Voting.Web.Controllers
{
    public class ResetController
    {
        private readonly IHubContext<VotingHub> context;

        public ResetController(IHubContext<VotingHub> context)
        {
            this.context = context;
        }

        [HttpPost("api/reset")]
        public Task Post()
        {
            return VotingHub.Reset(context);
        }
    }
}