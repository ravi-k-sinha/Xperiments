namespace Xperiments.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
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