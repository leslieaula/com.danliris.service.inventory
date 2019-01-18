using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Models.InventoryModel;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventorySummary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationServices.InventorySummaries
{
    public class InventorySummaryMigrationService : IInventorySummaryMigrationService
    {
        private readonly IInventorySummaryMongoRepository _mongoRepository;
        private readonly InventoryDbContext _dbContext;
        private readonly DbSet<InventorySummary> _inventorySummaryDbSet;
        
        public InventorySummaryMigrationService(IInventorySummaryMongoRepository mongoRepository, InventoryDbContext dbContext )
        {
            _mongoRepository = mongoRepository;
            _dbContext = dbContext;
            _inventorySummaryDbSet = dbContext.Set<InventorySummary>();
        }

        public int TotalInsertedData { get; private set; } = 0;

        public async Task<int> RunAsync(int startingNumber, int numberOfBatch)
        {
            var extractedData = await _mongoRepository.GetByBatch(startingNumber, numberOfBatch);

            if (extractedData.Count() > 0)
            {
                var transformedData = Transform(extractedData);
                startingNumber += transformedData.Count;

                //Insert into SQL
                Load(transformedData);
                TotalInsertedData += transformedData.Count;

                await RunAsync(startingNumber, numberOfBatch);
            }

            return TotalInsertedData;
        }

        private List<InventorySummary> Transform(IEnumerable<InventorySummaryMongo> extractedData)
        {
            return extractedData.Select(mongoInventoryDocument => new InventorySummary(mongoInventoryDocument)).ToList();
        }

        private int Load(List<InventorySummary> transformedData)
        {
            var existingUids = _inventorySummaryDbSet.Select(entity => entity.No).ToList();
            transformedData = transformedData.Where(entity => !_inventorySummaryDbSet
                                                                    .Any(x => x.No == entity.No
                                                                         && x.ProductCode == entity.ProductCode
                                                                         && x.StorageCode == entity.StorageCode
                                                                         && x.UomUnit == entity.UomUnit
                                                                         && x._IsDeleted == entity._IsDeleted
                                                                         && x.Active == entity.Active
                                                                         && x.Quantity == entity.Quantity
                                                                         && x.StockPlanning == entity.StockPlanning)).ToList();
            if (transformedData.Count > 0)
            {
                _inventorySummaryDbSet.AddRange(transformedData);
            }
            return _dbContext.SaveChanges();
        }
    }
}
