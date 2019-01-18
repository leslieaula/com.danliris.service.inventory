using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventorySummary
{
    public interface IInventorySummaryMongoRepository
    {
        Task<IEnumerable<InventorySummaryMongo>> GetByBatch(int startingNumber, int numberOfBatch);
    }
}
