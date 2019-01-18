using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Inventory.External.MicroService.Lib.MongoMasterResult
{
    public class ProductResult : BaseResult
    {
        public ProductResult()
        {
            data = new List<Product>();
        }
        public IList<Product> data { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
