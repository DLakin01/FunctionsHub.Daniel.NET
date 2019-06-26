using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoApi.Models;

namespace MongoApi
{
    public interface IRegulationRepository
    {
        Task<IEnumerable<Regulation>> GetAllRegulations(string collection);
        Task<IEnumerable<Regulation>> GetAllRegulationsInJurisdiction(string collection, string jurisdiction);
        Task<Regulation> GetRegulation(string collection, string id);

        // query multiple params
        Task<IEnumerable<Regulation>> GetRegulation(string collection, string title, string jurisdiction);

        // add new regulation
        Task AddRegulation(string collection, Regulation item);

        // add many regulations
        Task AddManyRegulations(string collection, IEnumerable<Regulation> items);

        // remove a single regulation
        Task<bool> RemoveRegulation(string collection, string id);

        // update a single regulation
        Task<bool> UpdateRegulation(string collection, string id, string text);
    }
}
