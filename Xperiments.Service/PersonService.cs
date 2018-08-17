using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xperiments.Models;
using Xperiments.Repository;

namespace Xperiments.Service
{
    public class PersonService : IPersonService
    {
        private IPersonRepository PersonRepository { get; set; }

        public PersonService(IPersonRepository personRepository)
        {
            PersonRepository = personRepository;
        }


        public async Task<List<IPerson>> GetAll()
        {
            return new List<IPerson>(await PersonRepository.All(p => true));
        }

        public async Task<IPerson> Get(string id)
        {
            return await PersonRepository.Get(id);
        }

        public async Task<List<IPerson>> GetByName(string name)
        {
            return new List<IPerson>(await PersonRepository.All(p => p.Name == name));
        }

        public Task<bool> Add(IPerson person)
        {
            return Task.Run(() =>
            {
                PersonRepository.Add(person);
                return true;
            });
        }
    }
}