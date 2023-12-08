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
    public async Task<List<Warehouse>> GetAll() => await _warehouseService.GetAllAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Warehouse>> GetById(string id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse is null)
        {
            return NotFound();
        }
        return warehouse;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Warehouse newWarehouse)
    {
        await _warehouseService.CreateAsync(newWarehouse);
        return CreatedAtAction(nameof(GetById), new { id = newWarehouse.Id }, newWarehouse);

    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Warehouse updatedWarehouse)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        if (warehouse is null)
        {
            return NotFound();
        }
        updatedWarehouse.Id = warehouse.Id;
        await _warehouseService.UpdateAsync(id, updatedWarehouse);

        return AcceptedAtAction(nameof(GetById), new { id = updatedWarehouse.Id }, updatedWarehouse);
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
