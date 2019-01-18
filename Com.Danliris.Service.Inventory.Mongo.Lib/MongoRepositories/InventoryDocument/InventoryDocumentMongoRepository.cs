using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try
            {
                return await _context
                           .InventoryDocuments
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
