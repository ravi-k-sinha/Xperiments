using System.Collections.Generic;
using System.Threading.Tasks;
using LendFoundry.Foundation.Logging;
using Xperiments.Models;
using Xperiments.Persistence.Common.Multilingual;
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

        [HttpPut]
        [Consumes("application/json")]
        public async Task<IActionResult> Update([FromBody] Person person)
        {
            return await Task.Run(async () => 
                Ok(PersonService.Update(person))
            );
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IPerson> Get([FromRoute] string id, [FromQuery] string locale)
        {
            return await Task.Run(async () =>
                {
                    if (! string.IsNullOrWhiteSpace(locale))
                    {
                        return await PersonService.GetByLocale(id, locale);
                    }

                    return await PersonService.Get(id);
                }
            );
        }
        
        [HttpGet]
        [Produces("application/json")]
        public async Task<List<IPerson>> Get()
        {
            return await Task.Run(async () => 
                await PersonService.GetAll()
            );
        }
        
        [HttpDelete("{id}")]
        [Produces("application/json")]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await Task.Run(async () => 
                await PersonService.Delete(id)
            );
        }

        [HttpPost("{id}/translation")]
        [Produces("application/json")]
        public async Task<bool> AddTranslation([FromRoute]string id, [FromBody] MultilingualDataRequest request)
        {
            return await Task.Run(async () => 
                await PersonService.AddTranslation(id, request)
            );
        }
    }
}