using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using System.Web.Http.ValueProviders;
using System.Web.UI.WebControls;
using WebApiPaging.Infrastructure;
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

        [HttpGet]
        [Route("customers/headers")]
        public HttpResponseMessage GetViaHeaders([ValueProvider(typeof(XHeaderValueProviderFactory))] int pageNo = 1, [ValueProvider(typeof(XHeaderValueProviderFactory))] int pageSize = 50)
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

            // Determine page count
            int pageCount = total > 0
                ? (int) Math.Ceiling(total/(double) pageSize)
                : 0;

            // Create the response
            var response = Request.CreateResponse(HttpStatusCode.OK, customers);

            // Set headers for paging
            response.Headers.Add("X-Paging-PageNo", pageNo.ToString());
            response.Headers.Add("X-Paging-PageSize", pageSize.ToString());
            response.Headers.Add("X-Paging-PageCount", pageCount.ToString());
            response.Headers.Add("X-Paging-TotalRecordCount", total.ToString());

            // Return the response
            return response;
        }
    }

}