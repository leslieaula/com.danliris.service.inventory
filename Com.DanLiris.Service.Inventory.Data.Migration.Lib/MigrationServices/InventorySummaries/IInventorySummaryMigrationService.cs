using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationServices.InventorySummaries
{
    public interface IInventorySummaryMigrationService
    {
        Task<int> RunAsync(int startingNumber, int numberOfBatch);
    }
}
