using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Models.InventoryModel;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventoryMovement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationServices.InventoryMovements
{
    public class InventoryMovementMigrationService : IInventoryMovementMigrationService
    {
        private readonly IInventoryMovementMongoRepository _mongoRepository;
        private readonly InventoryDbContext _dbContext;
        private readonly DbSet<InventoryMovement> _inventoryMovementDbSet;
        
        public InventoryMovementMigrationService(IInventoryMovementMongoRepository mongoRepository, InventoryDbContext dbContext)
        {
            _mongoRepository = mongoRepository;
            _dbContext = dbContext;
            _inventoryMovementDbSet = dbContext.Set<InventoryMovement>();
        }
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

        public int TotalInsertedData { get; private set; } = 0;

        private List<InventoryMovement> Transform(IEnumerable<InventoryMovementMongo> extractedData)
        {
            return extractedData.Select(mongoInventoryMovement => new InventoryMovement(mongoInventoryMovement)).ToList();
        }

        private int Load(List<InventoryMovement> transformedData)
        {
            var existingUids = _inventoryMovementDbSet.Select(entity => entity.No).ToList();
            transformedData = transformedData.Where(entity => !_inventoryMovementDbSet
                                                                    .Any(x => x.No == entity.No
                                                                         && x.ProductCode == entity.ProductCode
                                                                         && x.ReferenceNo == entity.ReferenceNo
                                                                         && x.ReferenceType == entity.ReferenceType
                                                                         && x.StorageCode == entity.StorageCode
                                                                         && x.Type == entity.Type
                                                                         && x.UomUnit == entity.UomUnit
                                                                         && x._IsDeleted == entity._IsDeleted
                                                                         && x.Active ==  entity.Active
                                                                         && x.Date == entity.Date
                                                                         && x.Quantity == entity.Quantity
                                                                         && x.StockPlanning == entity.StockPlanning)).ToList();
            if (transformedData.Count > 0)
            {
                _inventoryMovementDbSet.AddRange(transformedData);
            }
            return _dbContext.SaveChanges();
        }
    }
}
