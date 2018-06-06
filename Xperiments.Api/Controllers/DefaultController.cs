using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Xperiments.Api.Controllers
{
    [Route("/")]
    public class DefaultController : Controller
    {
        /// <summary>
        /// A default endpoint that returns welcome message
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return "Welcome to Xperiments!";
        }
    }
}