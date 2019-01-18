using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Inventory.External.MicroService.Lib.MongoMasterResult
{
    public class StorageResult : BaseResult
    {
        public StorageResult()
        {
            data = new List<Storage>();
        }
        public IList<Storage> data;
    }

    public class Storage
    {
        public int _id { get; set; }
        public string Code { get; set; }
    }
}
