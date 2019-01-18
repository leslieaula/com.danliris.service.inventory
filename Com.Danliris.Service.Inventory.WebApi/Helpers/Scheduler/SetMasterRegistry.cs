using Com.DanLiris.Service.Inventory.External.MicroService.Lib;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Danliris.Service.Inventory.WebApi.Helpers.Scheduler
{
    public class SetMasterRegistry : Registry
    {
        public SetMasterRegistry(IServiceProvider serviceProvider)
        {
            Schedule(() =>
            {

                var coreClient = serviceProvider.GetService<ICoreClient>();
                coreClient.SetProduct();
                coreClient.SetUom();
                coreClient.SetStorage();

            }).ToRunNow().AndEvery(1).Hours();
        }
    }
}
