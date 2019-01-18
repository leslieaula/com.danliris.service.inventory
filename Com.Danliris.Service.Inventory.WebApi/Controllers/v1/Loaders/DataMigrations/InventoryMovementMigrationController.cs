using Com.DanLiris.Service.Inventory.Data.Migration.Lib.MigrationServices.InventoryMovements;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.WebApi.Controllers.v1.Loaders.DataMigrations
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/migration/inventory-movements")]
    public class InventoryMovementMigrationController : Controller
    {
        private readonly IInventoryMovementMigrationService _service;

        public InventoryMovementMigrationController(IInventoryMovementMigrationService service)
        {
            _service = service;
        }

        [HttpGet("{startingNumber}/{numberOfBatch}")]
        public async Task<IActionResult> Get([FromRoute] int startingNumber, [FromRoute] int numberOfBatch)
        {
            try
            {
                var result = await _service.RunAsync(startingNumber, numberOfBatch);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
