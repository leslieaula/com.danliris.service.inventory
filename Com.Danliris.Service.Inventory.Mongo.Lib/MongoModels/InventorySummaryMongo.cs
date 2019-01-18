using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels
{
    public class InventorySummaryMongo : MongoBaseModel
    {
        public ObjectId productId { get; set; }

        public string productCode { get; set; }

        public string productName { get; set; }

        public ObjectId storageId { get; set; }

        public string storageCode { get; set; }

        public string storageName { get; set; }

        public int quantity { get; set; }

        public ObjectId uomId { get; set; }

        public string uom { get; set; }

        public int stockPlanning { get; set; }

        public string code { get; set; }
    }
}
