using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Inventory.Mongo.Lib
{
    public interface IMongoDbContext
    {
        IMongoCollection<InventoryDocumentMongo> InventoryDocuments { get; }
        IMongoCollection<InventoryMovementMongo> InventoryMovements { get; }
        IMongoCollection<InventorySummaryMongo> InventorySummaries { get; }
    }
}
