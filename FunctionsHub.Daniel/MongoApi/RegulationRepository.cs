using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoApi.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoApi
{
    public class RegulationRepository : IRegulationRepository
    {
        private readonly RegulationContext _context = null;

        public RegulationRepository(IOptions<Settings> settings)
        {
            _context = new RegulationContext(settings);
        }

        public async Task<IEnumerable<Regulation>> GetAllRegulations(string collection)
        {
            try
            {
                return await _context.Regulations(collection).Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Regulation>> GetAllRegulationsInJurisdiction(string collection, string jurisdiction)
        {
            try
            {
                var filter = Builders<Regulation>.Filter.Eq(s => s.jurisdiction, jurisdiction);
                return await _context.Regulations(collection).Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<Regulation> GetRegulation(string collection, string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.Regulations(collection).Find(r => r.Id == id || r.InternalId == internalId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Regulation>> GetRegulation(string collection, string title, string jurisdiction)
        {
            try
            {
                var query = _context.Regulations(collection).Find(r => r.RegTitle == title && r.jurisdiction == jurisdiction);
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddRegulation(string collection, Regulation item)
        {
            try
            {
                await _context.Regulations(collection).InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddManyRegulations(string collection, IEnumerable<Regulation> items)
        {
            try
            {
                await _context.Regulations(collection).InsertManyAsync(items);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RemoveRegulation(string collection, string id)
        {
            try
            {
                DeleteResult actionResult = await _context.Regulations(collection).DeleteOneAsync(Builders<Regulation>.Filter.Eq("Id", id));
                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateRegulation(string collection, string id, string text)
        {
            var filter = Builders<Regulation>.Filter.Eq(s => s.Id, id);
            var update = Builders<Regulation>.Update.Set(s => s.RegText, text).CurrentDate(s => s.UpdatedOn);

            try
            {
                UpdateResult actionResult = await _context.Regulations(collection).UpdateOneAsync(filter, update);
                return actionResult.IsAcknowledged && actionResult.MatchedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }
    }
}
