using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Inventory.External.MicroService.Lib
{
    public interface ICoreClient
    {
        void SetStorage();
        void SetProduct();
        void SetUom();
    }
}
