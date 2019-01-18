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
    [Route("v{version:apiVersion}/migration/inventory-documents-integrations")]
    public class InventoryDocumentIntegrationMigrationController : Controller
    {
        private readonly IInventoryDocumentIntegrationMigrationService _service;

        public InventoryDocumentIntegrationMigrationController(IInventoryDocumentIntegrationMigrationService service)
        {
            _service = service;
        }

        [HttpGet("set-headers")]
        public async Task<IActionResult> SetHeaders()
        {
            try
            {
                var result = await _service.SetMissingIdsOnHeader();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("set-items")]
        public async Task<IActionResult> SetItems()
        {
            try
            {
                var result = await _service.SetMissingIdsOnItem();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
