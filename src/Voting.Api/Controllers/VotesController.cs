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
                if (vote == "cats")
                {
                    results.Cats += 1;
                }
                else if (vote == "dogs")
                {
                    results.Dogs += 1;
                }
            }
            return results;
        }
        
        [HttpDelete]
        public VotingResults Delete()
        {
            lock (lockObject)
            {
                results.Dogs = results.Cats = 0;
            }
            return results;
        }
    }

    public class VotingResults
    {
        public int Cats { get; set; } = 0;
        public int Dogs { get; set; } = 0;
    }
}
