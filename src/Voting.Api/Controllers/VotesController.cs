using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Voting.Api.Controllers
{
    [Route("api/[controller]")]
    public class VotesController : Controller
    {
        private static VotingResults results = new VotingResults();
        private static object lockObject = new object();
        
        [HttpGet]
        public VotingResults Get()
        {
            return results;
        }

        [HttpPost]
        public VotingResults Post([FromQuery]string vote)
        {
            lock (lockObject)
            {
                if (vote == "dogs")
                {
                    results.Dogs += 1;
                }
                else if (vote == "bunnies")
                {
                    results.Bunnies += 1;
                }
                else if (vote == "horses")
                {
                    results.Horses += 1;
                }
            }
            return results;
        }
        
        [HttpDelete]
        public VotingResults Delete()
        {
            lock (lockObject)
            {
                results.Dogs = results.Bunnies = results.Horses = 0;
            }
            return results;
        }
    }

    public class VotingResults
    {
        public int Dogs { get; set; } = 0;
        public int Bunnies { get; set; } = 0;
        public int Horses { get; set; } = 0;
    }
}
