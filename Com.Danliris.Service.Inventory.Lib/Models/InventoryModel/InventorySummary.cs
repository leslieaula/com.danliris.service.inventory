using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Inventory.Lib.Models.InventoryModel
{
    public class InventorySummary : StandardEntity
    {
        public InventorySummary()
        {

        }

        public InventorySummary(InventorySummaryMongo mongoInventorySummary)
        {
            Active = mongoInventorySummary._active;
            UomUnit = mongoInventorySummary.uom;
            StorageCode = mongoInventorySummary.storageCode;
            StorageName = mongoInventorySummary.storageName;
            StockPlanning = mongoInventorySummary.stockPlanning;
            Quantity = mongoInventorySummary.quantity;
            ProductCode = mongoInventorySummary.productCode;
            ProductName = mongoInventorySummary.productName;
            No = mongoInventorySummary.code;
            _CreatedAgent = mongoInventorySummary._createAgent;
            _CreatedBy = mongoInventorySummary._createdBy;
            _CreatedUtc = mongoInventorySummary._createdDate;
            _DeletedAgent = mongoInventorySummary._deleted ? mongoInventorySummary._updateAgent : "";
            _DeletedBy = mongoInventorySummary._deleted ? mongoInventorySummary._updatedBy : "";
            _DeletedUtc = mongoInventorySummary._deleted ? mongoInventorySummary._updatedDate : DateTime.MinValue;
            _IsDeleted = mongoInventorySummary._deleted;
            _LastModifiedAgent = mongoInventorySummary._updateAgent;
            _LastModifiedBy = mongoInventorySummary._updatedBy;
            _LastModifiedUtc = mongoInventorySummary._updatedDate;
        }

        public string No { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public string UomUnit { get; set; }
        public string UomId { get; set; }

        public int StorageId { get; set; }
        public string StorageCode { get; set; }
        public string StorageName { get; set; }
        public double Quantity { get; set; }
        public double StockPlanning { get; set; }
    }
}