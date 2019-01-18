using MongoDB.Bson;
using System;

namespace Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels
{
    public class InventoryMovementMongo : MongoBaseModel
    {
        public string code { get; set; }

        public string referenceNo { get; set; }

        public string referenceType { get; set; }

        public DateTime date { get; set; }

        public ObjectId productId { get; set; }

        public string productCode { get; set; }

        public string productName { get; set; }

        public ObjectId storageId { get; set; }

        public string storageCode { get; set; }

        public string storageName { get; set; }

        public double stockPlanning { get; set; }

        public double before { get; set; }

        public double quantity { get; set; }

        public double after { get; set; }

        public ObjectId uomId { get; set; }

        public string uom { get; set; }

        public string remark { get; set; }

        public string type { get; set; }
    }
}
