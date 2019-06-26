using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoApi;
using MongoApi.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace WebApplication1.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RegulationsController : Controller
    {
        private readonly IRegulationRepository _regulationRepository;

        public RegulationsController(IRegulationRepository regulationRepository)
        {
            _regulationRepository = regulationRepository;
        }

        [HttpGet("{collection}")]
        // GET api/regulations
        public async Task<IEnumerable<Regulation>> Get(string collection)
        {
            return await _regulationRepository.GetAllRegulations(collection);
        }

        // GET api/regulations/5
        [HttpGet("{collection}/id/{id}")]
        public async Task<Regulation> GetRegulationById(string collection, string id)
        {
            return await _regulationRepository.GetRegulation(collection, id) ?? new Regulation();
        }

        // GET api/regulations/{jurisdiction}
        [HttpGet("{collection}/jurisdiction/{jurisdiction}")]
        public async Task<IEnumerable<Regulation>> GetRegulationsInJurisdiction(string collection, string jurisdiction)
        {
            return await _regulationRepository.GetAllRegulationsInJurisdiction(collection, jurisdiction);
        }

        // POST api/regulation
        [HttpPost("{collection}/add")]
        public void Post(string collection, [FromBody] IEnumerable<Regulation> newRegulations)
        {
            _regulationRepository.AddManyRegulations(collection, newRegulations);
        }

        // PUT api/values/5
        [HttpPut("{collection}/{id}")]
        public void Put(string collection, string id, [FromBody]string value)
        {
            _regulationRepository.UpdateRegulation(collection, id, value);
        }

        // DELETE api/values/5
        [HttpDelete("{collection}/{id}")]
        public void Delete(string collection, string id)
        {
            _regulationRepository.RemoveRegulation(collection, id);
        }
    }
}
