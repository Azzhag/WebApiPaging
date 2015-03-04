using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiPaging.Models;

namespace WebApiPaging.Controllers
{
    [RoutePrefix("api")]
    public class TweetsController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;

        public TweetsController()
        {
            _dbContext = new ApplicationDbContext();   
        }

        [HttpGet]
        [Route("tweets")]
        public IHttpActionResult GetTweets(int count = 50, int? after = null, int? before = null)
        {
            List<Tweet> tweets;

            if (after.HasValue)
            {
                // if the user specified the after parameter, return the next batch of records with a higher ID
                tweets = _dbContext.Tweets
                    .Where(t => t.Id > after)
                    .OrderBy(t => t.Id)
                    .Take(count)
                    .ToList();
            }
            else if (before.HasValue)
            {
                // if the user specified the before parameter, return the next batch of records with a lower ID
                tweets = _dbContext.Tweets
                    .Where(t => t.Id < before)
                    .OrderByDescending(t => t.Id)
                    .Take(count)
                    .ToList();
            }
            else
            {
                // If neither is specified return the latest records
                tweets = _dbContext.Tweets
                    .OrderByDescending(t => t.Id)
                    .Take(count)
                    .ToList();
            }

            return Ok(tweets.OrderBy(t => t.Id));
        }
    }
}