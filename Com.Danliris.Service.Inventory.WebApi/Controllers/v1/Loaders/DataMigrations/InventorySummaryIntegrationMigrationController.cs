using Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationIntegrationServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.WebApi.Controllers.v1.Loaders.DataMigrations
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/migration/inventory-summaries-integrations")]
    public class InventorySummaryIntegrationMigrationController : Controller
    {
        private readonly IInventorySummaryIntegrationMigrationService _service;

        public InventorySummaryIntegrationMigrationController(IInventorySummaryIntegrationMigrationService service)
        {
            _service = service;
        }

        [HttpGet("set")]
        public async Task<IActionResult> SetHeaders()
        {
            try
            {
                var result = await _service.SetMissingIds();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
