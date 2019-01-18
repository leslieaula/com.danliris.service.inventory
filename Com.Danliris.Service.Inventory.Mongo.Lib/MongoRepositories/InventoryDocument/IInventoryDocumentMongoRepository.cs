using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventoryDocument
{
    public interface IInventoryDocumentMongoRepository
    {
        Task<IEnumerable<InventoryDocumentMongo>> GetByBatch(int startingNumber, int numberOfBatch);
    }
}
