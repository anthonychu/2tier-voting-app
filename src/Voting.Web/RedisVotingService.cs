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
            if (vote != "dogs" && vote != "cats")
            {
                throw new ArgumentException("Must be dogs or cats");
            }

            await db.HashIncrementAsync("votes", vote, 1);
            return await GetResultsAsync();
        }

        public async Task<VotingResults> GetResultsAsync()
        {
            var results = (await db.HashGetAllAsync("votes")).ToStringDictionary();
            return new VotingResults
            {
                Cats = results.ContainsKey("cats") ? Convert.ToInt32(results["cats"]) : 0,
                Dogs = results.ContainsKey("dogs") ? Convert.ToInt32(results["dogs"]) : 0
            };
        }

        public async Task<VotingResults> ResetResultsAsync()
        {
            await db.HashSetAsync("votes", new HashEntry[]
            {
                new HashEntry("cats", 0),
                new HashEntry("dogs", 0)
            });
            return new VotingResults();
        }
    }
}