using Microsoft.AspNetCore.Mvc;
using Products.Models.Entities;
using Products.Services;

namespace Products.Controllers;

[ApiController]
[Route("internal/api/v1/warehouse/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly WarehouseService _warehouseService;

    public WarehouseController(WarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    public async Task<List<WarehouseDb>> GetAll() => await _warehouseService.GetAllAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<WarehouseDb>> GetById(string id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse is null)
        {
            return NotFound();
        }
        return warehouse;
    }

    [HttpPost]
    public async Task<IActionResult> Post(WarehouseDb newWarehouseDb)
    {
        await _warehouseService.CreateAsync(newWarehouseDb);
        return CreatedAtAction(nameof(GetById), new { id = newWarehouseDb.Id }, newWarehouseDb);

    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, WarehouseDb updatedWarehouseDb)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse is null)
        {
            return NotFound();
        }
        updatedWarehouseDb.Id = warehouse.Id;
        await _warehouseService.UpdateAsync(id, updatedWarehouseDb);

        return AcceptedAtAction(nameof(GetById), new { id = updatedWarehouseDb.Id }, updatedWarehouseDb);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Remove(string id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse is null)
        {
            return NotFound();
        }
        await _warehouseService.RemoveAsync(id);
        return NoContent();
    }
}
