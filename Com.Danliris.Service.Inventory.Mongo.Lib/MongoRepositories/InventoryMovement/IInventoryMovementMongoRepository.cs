using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventoryMovement
{
    public interface IInventoryMovementMongoRepository
    {
        Task<IEnumerable<InventoryMovementMongo>> GetByBatch(int startingNumber, int numberOfBatch);
    }
}
