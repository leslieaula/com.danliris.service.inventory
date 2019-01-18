using System.Threading.Tasks;

namespace Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationServices.InventoryDocuments
{
    public interface IInventoryDocumentMigrationService
    {
        Task<int> RunAsync(int startingNumber, int numberOfBatch);
    }
}
