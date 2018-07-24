using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            //Logger.Info("Get() called");
            //Logger.Info("Get() Exited", new { myvar});
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Another sample endpoint that accepts information through payload
        /// </summary>
        /// <param name="sampleBody"></param>
        /// <returns></returns>
        [HttpPost("sample")]
        [Produces("application/json")]
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
        public async Task<string> Get(int id)
        {
            return await Task.Run(() => $"You passed {id}");
            //return "A Return Value";
        }

        // POST api/values
        [HttpPost]
        public async Task<object> Post(string value)
        {
            return await Task.Run(() => new {
                value,
                message = "Message from a Post endpoint. Your request was processed"
            });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]SampleBody value)
        {
            return;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("action-result")]
        public async Task<IActionResult> GetActionResult()
        {
            return await Task.Run(async () =>
            {
                return this.Ok(new List<List<string>> { new List<string> { "ar1", "ar2" } , new List<string> { "ar3", "ar4" }});
            });
        }
    }

    public class SampleBody
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
