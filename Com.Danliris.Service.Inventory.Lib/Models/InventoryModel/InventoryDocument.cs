using Com.Danliris.Service.Inventory.Mongo.Lib.MongoModels;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Inventory.Lib.Models.InventoryModel
{
    public class InventoryDocument :  StandardEntity, IValidatableObject
    {
        public InventoryDocument()
        {
               
        }

        public InventoryDocument(InventoryDocumentMongo mongoInventoryDocument)
        {
            Active = mongoInventoryDocument._active;
            Date = mongoInventoryDocument.date;
            No = mongoInventoryDocument.code;
            ReferenceNo = mongoInventoryDocument.referenceNo;
            ReferenceType = mongoInventoryDocument.referenceType;
            Remark = mongoInventoryDocument.remark;
            StorageCode = mongoInventoryDocument.storageCode;
            StorageName = mongoInventoryDocument.storageName;
            Type = mongoInventoryDocument.type;
            _CreatedAgent = mongoInventoryDocument._createAgent;
            _CreatedBy = mongoInventoryDocument._createdBy;
            _CreatedUtc = mongoInventoryDocument._createdDate;
            _DeletedAgent = mongoInventoryDocument._deleted ? mongoInventoryDocument._updateAgent : "";
            _DeletedBy = mongoInventoryDocument._deleted ? mongoInventoryDocument._updatedBy : "";
            _DeletedUtc = mongoInventoryDocument._deleted ? mongoInventoryDocument._updatedDate : DateTime.MinValue;
            _IsDeleted = mongoInventoryDocument._deleted;
            _LastModifiedAgent = mongoInventoryDocument._updateAgent;
            _LastModifiedBy = mongoInventoryDocument._updatedBy;
            _LastModifiedUtc = mongoInventoryDocument._updatedDate;
            Items = mongoInventoryDocument.items.Select(mongoInventoryDocumentItem => new InventoryDocumentItem(mongoInventoryDocumentItem)).ToList();
        }

        public string No { get; set; }
        public DateTimeOffset Date { get; set; }
        public string ReferenceNo { get; set; }
        public string ReferenceType { get; set; }
        public int StorageId { get; set; }
        public string StorageCode { get; set; }
        public string StorageName { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }
        public virtual ICollection<InventoryDocumentItem> Items { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
