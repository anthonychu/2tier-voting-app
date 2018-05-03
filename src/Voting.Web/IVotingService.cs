using System.Threading.Tasks;

namespace Voting.Web
{
    public interface IVotingService
    {
        Task<VotingResults> VoteAsync(string vote);
        Task<VotingResults> GetResultsAsync();
        Task<VotingResults> ResetResultsAsync();
    }
}