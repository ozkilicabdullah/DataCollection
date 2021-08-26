using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkController : ControllerBase
    {

        // POST api/values
        [HttpPost]
        [Route("Products")]
        public void ProductsInsert([FromBody] string value)
        {
        }
    }
}
