using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventorySummary
{
    public class InventorySummaryMongoRepository : IInventorySummaryMongoRepository
    {
        private readonly IMongoDbContext _context;

        public InventorySummaryMongoRepository(IMongoDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<InventorySummaryMongo>> GetByBatch(int startingNumber, int numberOfBatch)
        {
            try
            {
                return await _context
                           .InventorySummaries
                           .AsQueryable()
                           .Skip(startingNumber)
                           .Take(numberOfBatch)
                           .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
