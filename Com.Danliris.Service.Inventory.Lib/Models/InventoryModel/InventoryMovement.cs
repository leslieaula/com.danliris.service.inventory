using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Inventory.Lib.Models.InventoryModel
{
    public class InventoryMovement : StandardEntity
    {
        public InventoryMovement()
        {

        }

        public InventoryMovement(InventoryMovementMongo mongoInventoryMovement)
        {
            Active = mongoInventoryMovement._active;
            After = mongoInventoryMovement.after;
            Before = mongoInventoryMovement.before;
            Date = mongoInventoryMovement.date;
            No = mongoInventoryMovement.code;
            ProductCode = mongoInventoryMovement.productCode;
            ProductName = mongoInventoryMovement.productName;
            Quantity = mongoInventoryMovement.quantity;
            ReferenceNo = mongoInventoryMovement.referenceNo;
            ReferenceType = mongoInventoryMovement.referenceType;
            Remark = mongoInventoryMovement.remark;
            StockPlanning = mongoInventoryMovement.stockPlanning;
            StorageCode = mongoInventoryMovement.storageCode;
            StorageName = mongoInventoryMovement.storageName;
            Type = mongoInventoryMovement.type;
            UomUnit = mongoInventoryMovement.uom;
            _CreatedAgent = mongoInventoryMovement._createAgent;
            _CreatedBy = mongoInventoryMovement._createdBy;
            _CreatedUtc = mongoInventoryMovement._createdDate;
            _DeletedAgent = mongoInventoryMovement._deleted ? mongoInventoryMovement._updateAgent : "";
            _DeletedBy = mongoInventoryMovement._deleted ? mongoInventoryMovement._updatedBy : "";
            _DeletedUtc = mongoInventoryMovement._deleted ? mongoInventoryMovement._updatedDate : DateTime.MinValue;
            _IsDeleted = mongoInventoryMovement._deleted;
            _LastModifiedAgent = mongoInventoryMovement._updateAgent;
            _LastModifiedBy = mongoInventoryMovement._updatedBy;
            _LastModifiedUtc = mongoInventoryMovement._updatedDate;
        }
        public string No { get; set; }
        public DateTimeOffset Date { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceType { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public string UomUnit { get; set; }
        public string UomId { get; set; }

        public int StorageId { get; set; }
        public string StorageCode { get; set; }
        public string StorageName { get; set; }

        public double StockPlanning { get; set; }

        public double Before { get; set; }
        public double Quantity { get; set; }
        public double After { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }
    }
}