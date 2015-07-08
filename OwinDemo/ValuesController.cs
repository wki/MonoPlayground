using System;
using System.Web.Http;
using System.Collections.Generic;

namespace OwinDemo
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [Route("")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/all
        [Route("all")]
        [HttpGet]
        public IEnumerable<string> All()
        {
            return new[] { "v1", "v2", "v3", "v4" };
        }
    }
}
