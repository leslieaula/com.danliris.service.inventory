using System.Collections.Generic;

namespace Com.DanLiris.Service.Inventory.External.MicroService.Lib.MongoMasterResult
{
    public class UomResult : BaseResult
    {
        public UomResult()
        {
            data = new List<Uom>();
        }
        public IList<Uom> data { get; set; }
    }

    public class Uom
    {
        public int Id { get; set; }
        public string Unit { get; set; }
    }
}
