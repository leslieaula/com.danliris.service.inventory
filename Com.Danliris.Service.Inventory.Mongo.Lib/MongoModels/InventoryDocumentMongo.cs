using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels
{
    public class InventoryDocumentMongo : MongoBaseModel
    {
        public string code { get; set; }

        public DateTime date { get; set; }

        public string referenceNo { get; set; }

        public string referenceType { get; set; }

        public string type { get; set; }

        public ObjectId storageId { get; set; }

        public string storageCode { get; set; }

        public string storageName { get; set; }

        public string remark { get; set; }

        public List<InventoryDocumentItemMongo> items { get; set; }
    }
}
