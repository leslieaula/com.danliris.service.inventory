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
    public class InventoryMovementIntegrationMigrationService : IInventoryMovementIntegrationMigrationService
    {

        private readonly IMemoryCacheManager _cacheManager;
        private readonly InventoryDbContext _dbContext;
        private readonly DbSet<InventoryMovement> _inventoryMovementDbSet;

        public InventoryMovementIntegrationMigrationService(IMemoryCacheManager cacheManager, InventoryDbContext dbContext)
        {
            _cacheManager = cacheManager;
            _dbContext = dbContext;

            _inventoryMovementDbSet = dbContext.Set<InventoryMovement>();
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
            var inventoryMovements = _inventoryMovementDbSet.ToList();

            var inventoryMovementsToUpdate = new List<InventoryMovement>();

            foreach(var inventoryMovement in inventoryMovements)
            {
                var storage = Storages.FirstOrDefault(x => x.Code == inventoryMovement.StorageCode);
                if (storage == null)
                    continue;
                inventoryMovement.StorageId = storage._id;

                var product = Products.FirstOrDefault(x => x.Code == inventoryMovement.ProductCode);
                if (product == null)
                    continue;

                inventoryMovement.ProductId = product.Id.ToString();

                var uom = Uoms.FirstOrDefault(x => x.Unit == inventoryMovement.UomUnit);
                if (uom == null)
                    continue;

                inventoryMovement.UomId = uom.Id.ToString();

                inventoryMovementsToUpdate.Add(inventoryMovement);
            }

            _inventoryMovementDbSet.UpdateRange(inventoryMovementsToUpdate);

            return _dbContext.SaveChangesAsync();
        }
    }

    public interface IInventoryMovementIntegrationMigrationService
    {
        Task<int> SetMissingIds();
    }
}
