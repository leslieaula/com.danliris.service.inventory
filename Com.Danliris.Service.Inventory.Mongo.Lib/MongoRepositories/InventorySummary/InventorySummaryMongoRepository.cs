using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using MongoDB.Driver;
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
            return await _context
                           .InventorySummaries
                           .Find(x => !x._deleted)
                           .Skip(startingNumber)
                           .Limit(numberOfBatch)
                           .ToListAsync();
        }
    }
}
