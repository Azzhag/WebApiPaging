using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiPaging.Models;

namespace WebApiPaging.Controllers
{
    [RoutePrefix("api")]
    public class CustomersController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomersController()
        {
            _dbContext = new ApplicationDbContext();    
        }

        [HttpGet]
        [Route("customers")]
        public IHttpActionResult Get(int offset = 0, int limit = 50)
        {
            // Get total number of records
            int total = _dbContext.Customers.Count();

            // Select the customers based on paging parameters
            var customers = _dbContext.Customers
                .OrderBy(c => c.Id)
                .Skip(offset)
                .Take(limit)
                .ToList();

            // Return the list of customers
            return Ok(new
            {
                Data = customers,
                Paging = new
                {
                    Total = total,
                    Limit = limit,
                    Offset = offset,
                    Returned = customers.Count
                }
            });
        }


        [HttpGet]
        [Route("customers/paged")]
        public IHttpActionResult GetPaged(int pageNo = 1, int pageSize = 50)
        {
            // Determine the number of records to skip
            int skip = (pageNo - 1) * pageSize;

            // Get total number of records
            int total = _dbContext.Customers.Count();

            // Select the customers based on paging parameters
            var customers = _dbContext.Customers
                .OrderBy(c => c.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            // Return the list of customers
            return Ok(new PagedResult<Customer>(customers, pageNo, pageSize, total));
        }
    }
}