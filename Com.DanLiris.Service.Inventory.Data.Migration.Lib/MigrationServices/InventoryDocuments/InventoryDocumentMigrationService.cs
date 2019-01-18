using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Models.InventoryModel;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoRepositories.InventoryDocument;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationServices.InventoryDocuments
{
    public class InventoryDocumentMigrationService : IInventoryDocumentMigrationService
    {
        private readonly IInventoryDocumentMongoRepository _mongoRepository;
        private readonly InventoryDbContext _dbContext;
        private readonly DbSet<InventoryDocument> _inventoryDocumentDbSet;
        private readonly DbSet<InventoryDocumentItem> _inventoryDocumentItemDbSet;

        public InventoryDocumentMigrationService(IInventoryDocumentMongoRepository mongoRepository, InventoryDbContext dbContext)
        {
            _mongoRepository = mongoRepository;
            _dbContext = dbContext;
            _inventoryDocumentDbSet = dbContext.Set<InventoryDocument>();
            _inventoryDocumentItemDbSet = dbContext.Set<InventoryDocumentItem>();
        }

        public int TotalInsertedData { get; private set; } = 0;

        public async Task<int> RunAsync(int startingNumber, int numberOfBatch)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private List<InventoryDocument> Transform(IEnumerable<InventoryDocumentMongo> extractedData)
        {
            return extractedData.Select(mongoInventoryDocument => new InventoryDocument(mongoInventoryDocument)).ToList();
        }

        private int Load(List<InventoryDocument> transformedData)
        {
            var existingUids = _inventoryDocumentDbSet.Select(entity => entity.No).ToList();
            transformedData = transformedData.Where(entity => !existingUids.Contains(entity.No)).ToList();
            if (transformedData.Count > 0)
            {
                _inventoryDocumentItemDbSet.AddRange(transformedData.SelectMany(x => x.Items));
                _inventoryDocumentDbSet.AddRange(transformedData);
            }
            return _dbContext.SaveChanges();
        }
    }
}
