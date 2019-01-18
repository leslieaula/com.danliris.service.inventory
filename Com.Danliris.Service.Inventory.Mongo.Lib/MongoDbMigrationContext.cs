using System;
using System.Collections.Generic;
using System.Text;
using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Com.Danliris.Service.Inventory.Mongo.Lib
{
    public class MongoDbMigrationContext : IMongoDbContext
    {
        private readonly IMongoDatabase _db;

        public MongoDbMigrationContext(IOptions<MongoDbSettings> options, IMongoClient client)
        {
            _db = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<InventoryDocumentMongo> InventoryDocuments => _db.GetCollection<InventoryDocumentMongo>("inventory-documents");

        public IMongoCollection<InventoryMovementMongo> InventoryMovements => _db.GetCollection<InventoryMovementMongo>("inventory-movements");

        public IMongoCollection<InventorySummaryMongo> InventorySummaries => _db.GetCollection<InventorySummaryMongo>("inventory-summaries");
    }
}
