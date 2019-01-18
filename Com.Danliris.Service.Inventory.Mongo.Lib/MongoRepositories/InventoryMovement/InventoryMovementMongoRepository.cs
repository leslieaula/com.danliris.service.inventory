using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventoryMovement
{
    public class InventoryMovementMongoRepository : IInventoryMovementMongoRepository
    {
        private readonly IMongoDbContext _context;

        public InventoryMovementMongoRepository(IMongoDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<InventoryMovementMongo>> GetByBatch(int startingNumber, int numberOfBatch)
        {
            return await _context
                           .InventoryMovements
                           .Find(x => !x._deleted)
                           .Skip(startingNumber)
                           .Limit(numberOfBatch)
                           .ToListAsync();
        }
    }
}
