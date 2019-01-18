using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventoryDocument
{
    public class InventoryDocumentMongoRepository : IInventoryDocumentMongoRepository
    {
        private readonly IMongoDbContext _context;

        public InventoryDocumentMongoRepository(IMongoDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<InventoryDocumentMongo>> GetByBatch(int startingNumber, int numberOfBatch)
        {
            return await _context
                           .InventoryDocuments
                           .Find(x => !x._deleted)
                           .Skip(startingNumber)
                           .Limit(numberOfBatch)
                           .ToListAsync();
        }
    }
}
