using System.Collections.Generic;
using System.Threading.Tasks;
using LendFoundry.Foundation.Logging;
using Xperiments.Models;
using Xperiments.Service;

namespace Xperiments.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[controller]")]
    public class PersonController : Controller
    {        
        private ILogger Logger { get; }

        public PersonController(IPersonService personService, ILogger logger) {
            Logger = logger;
            PersonService = personService;
        }

        private IPersonService PersonService { get; }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] Person person)
        {
            return await Task.Run(async () => 
                Ok(PersonService.Add(person))
            );
        }
    }
}