using System;
using System.Collections.Generic;
using LendFoundry.Foundation.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Xperiments.Api.Controllers
{
    /// <summary>
    /// A simple controller to experiment ASP.NET features
    /// </summary>
    [Route("api/[controller]")]
    public class SimpleController : Controller
    {

        private ILogger Logger { get; }

        public SimpleController(ILogger logger) {
            Logger = logger;
        }

        /// <summary>
        /// Test Endpoint
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var myvar = "value1";
            Logger.Info("Get() called");
            Logger.Info("Get() Exited", new { myvar});
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Another sample endpoint that accepts information through payload
        /// </summary>
        /// <param name="sampleBody"></param>
        /// <returns></returns>
        [HttpPost("sample")]
        public string GetBody([FromBody]SampleBody sampleBody) {
            Logger.Info("GetBody() called");

            Logger.Info("GetBody() received", new {sampleBody});
            return "return";
        }

        [HttpGet("exception")]
        public string ExceptionThrower()
        {
            throw new InvalidOperationException("Dummy ExceptionThrower()");
        }

        [HttpGet("by-zero")]
        public string ExceptionCatcher()
        {
            var zero = 0;

            try
            {
                var v = 34 / zero;
            }
            catch (Exception e) {
                Logger.Error("Caught an exception", e);
                throw e;
            }

            return "Boom";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class SampleBody
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
