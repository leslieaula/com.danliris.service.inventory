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
    public class InventorySummaryIntegrationMigrationService : IInventorySummaryIntegrationMigrationService
    {
        private readonly IMemoryCacheManager _cacheManager;
        private readonly InventoryDbContext _dbContext;
        private readonly DbSet<InventorySummary> _inventorySummaryDbSet;

        public InventorySummaryIntegrationMigrationService(IMemoryCacheManager cacheManager, InventoryDbContext dbContext)
        {
            _cacheManager = cacheManager;
            _dbContext = dbContext;

            _inventorySummaryDbSet = dbContext.Set<InventorySummary>();
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
        public Task<int> SetMissingIds()
        {
            var inventorySummaries = _inventorySummaryDbSet.ToList();

            var inventorySummariesToUpdate = new List<InventorySummary>();

            foreach (var inventorySummary in inventorySummaries)
            {
                var storage = Storages.FirstOrDefault(x => x.Code == inventorySummary.StorageCode);
                if (storage == null)
                    continue;
                inventorySummary.StorageId = storage._id;

                var product = Products.FirstOrDefault(x => x.Code == inventorySummary.ProductCode);
                if (product == null)
                    continue;

                inventorySummary.ProductId = product.Id.ToString();

                var uom = Uoms.FirstOrDefault(x => x.Unit == inventorySummary.UomUnit);
                if (uom == null)
                    continue;

                inventorySummary.UomId = uom.Id.ToString();

                inventorySummariesToUpdate.Add(inventorySummary);
            }

            _inventorySummaryDbSet.UpdateRange(inventorySummariesToUpdate);

            return _dbContext.SaveChangesAsync();
        }
    }

    public interface IInventorySummaryIntegrationMigrationService
    {
        Task<int> SetMissingIds();
    }
}
