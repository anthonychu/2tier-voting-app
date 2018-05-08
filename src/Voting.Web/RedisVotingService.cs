using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Voting.Web
 {
    public class RedisVotingService : IVotingService
    {
        private readonly ConnectionMultiplexer redisConnection;
        private readonly IDatabase db;

        public RedisVotingService(ConnectionMultiplexer redisConnection)
        {
            this.redisConnection = redisConnection;
            this.db = redisConnection.GetDatabase();
        }

        public async Task<VotingResults> VoteAsync(string vote)
        {
            if (vote != "dogs" && vote != "bunnies" && vote != "horses")
            {
                throw new ArgumentException("Must be dogs, bunnies, or horses");
            }

            await db.HashIncrementAsync("votes", vote, 1);
            return await GetResultsAsync();
        }

        public async Task<VotingResults> GetResultsAsync()
        {
            var results = (await db.HashGetAllAsync("votes")).ToStringDictionary();
            return new VotingResults
            {
                Dogs = results.ContainsKey("dogs") ? Convert.ToInt32(results["dogs"]) : 0,
                Bunnies = results.ContainsKey("bunnies") ? Convert.ToInt32(results["bunnies"]) : 0,
                Horses = results.ContainsKey("horses") ? Convert.ToInt32(results["horses"]) : 0
            };
        }

        public async Task<VotingResults> ResetResultsAsync()
        {
            await db.HashSetAsync("votes", new HashEntry[]
            {
                new HashEntry("dogs", 0),
                new HashEntry("bunnies", 0),
                new HashEntry("horses", 0)
            });
            return new VotingResults();
        }
    }
}