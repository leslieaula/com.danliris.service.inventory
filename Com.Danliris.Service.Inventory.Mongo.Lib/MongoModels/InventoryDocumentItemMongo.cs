using MongoDB.Bson;

namespace Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels
{
    public class InventoryDocumentItemMongo : MongoBaseModel
    {
        public ObjectId productId { get; set; }

        public string productCode { get; set; }

        public string productName { get; set; }

        public int quantity { get; set; }

        public ObjectId uomId { get; set; }

        public string uom { get; set; }

        public int stockPlanning { get; set; }

        public string remark { get; set; }
    }
}
