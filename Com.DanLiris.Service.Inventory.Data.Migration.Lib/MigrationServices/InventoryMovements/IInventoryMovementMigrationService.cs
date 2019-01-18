using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationServices.InventoryMovements
{
    public interface IInventoryMovementMigrationService
    {
        Task<int> RunAsync(int startingNumber, int numberOfBatch);
    }
}
