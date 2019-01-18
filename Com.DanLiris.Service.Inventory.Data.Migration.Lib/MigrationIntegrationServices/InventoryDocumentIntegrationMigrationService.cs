using Com.Danliris.Service.Inventory.Lib;
using Com.Danliris.Service.Inventory.Lib.Models.InventoryModel;
using Com.DanLiris.Service.Inventory.External.MicroService.Lib.Cache;
using Com.DanLiris.Service.Inventory.External.MicroService.Lib.MongoMasterResult;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationIntegrationServices
{
    public class InventoryDocumentIntegrationMigrationService : IInventoryDocumentIntegrationMigrationService
    {
        private readonly IMemoryCacheManager _cacheManager;
        private readonly InventoryDbContext _dbContext;
        private readonly DbSet<InventoryDocument> _inventoryDocumentDbSet;
        private readonly DbSet<InventoryDocumentItem> _inventoryDocumentItemDbSet;

        public InventoryDocumentIntegrationMigrationService(IMemoryCacheManager cacheManager, InventoryDbContext dbContext)
        {
            _cacheManager = cacheManager;
            _dbContext = dbContext;

            _inventoryDocumentDbSet = dbContext.Set<InventoryDocument>();
            _inventoryDocumentItemDbSet = dbContext.Set<InventoryDocumentItem>();
        }

        private IList<Product> Products => _cacheManager.Get("Products", entry =>
        {
            return new List<Product>();
        });

        private IList<Uom> Uoms => _cacheManager.Get("Uoms", entry =>
        {
            return new List<Uom>();
        });

        private IList<Storage> Storages => _cacheManager.Get("Storages", entry =>
        {
            return new List<Storage>();
        });

        public Task<int> SetMissingIdsOnHeader()
        {
            var inventoryDocuments = _inventoryDocumentDbSet.ToList();

            var inventoryDocumentsToUpdate = new List<InventoryDocument>();

            foreach(var inventoryDocument in inventoryDocuments)
            {
                var storage = Storages.FirstOrDefault(x => x.Code == inventoryDocument.StorageCode);
                if (storage == null)
                    continue;

                inventoryDocument.StorageId = storage._id;
                inventoryDocumentsToUpdate.Add(inventoryDocument);
            }

            _inventoryDocumentDbSet.UpdateRange(inventoryDocumentsToUpdate);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> SetMissingIdsOnItem()
        {
            var inventoryDocumentItems = _inventoryDocumentItemDbSet.ToList();

            var inventoryDocumentItemsToUpdate = new List<InventoryDocumentItem>();

            foreach (var inventoryDocumentItem in inventoryDocumentItems)
            {
                var product = Products.FirstOrDefault(x => x.Code == inventoryDocumentItem.ProductCode);
                if (product == null)
                    continue;

                inventoryDocumentItem.ProductId = product.Id.ToString();

                var uom = Uoms.FirstOrDefault(x => x.Unit == inventoryDocumentItem.UomUnit);
                if (uom == null)
                    continue;

                inventoryDocumentItem.UomId = uom.Id.ToString();

                inventoryDocumentItemsToUpdate.Add(inventoryDocumentItem);
            }

            _inventoryDocumentItemDbSet.UpdateRange(inventoryDocumentItemsToUpdate);

            return _dbContext.SaveChangesAsync();
        }
    }

    public interface IInventoryDocumentIntegrationMigrationService
    {
        Task<int> SetMissingIdsOnHeader();
        Task<int> SetMissingIdsOnItem();
    }
}
