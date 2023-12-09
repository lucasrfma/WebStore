using Microsoft.AspNetCore.Mvc;
using Products.Models.Dtos;
using Products.Models.Entities;
using Products.Services;

namespace Products.Controllers;

[ApiController]
[Route("internal/api/v1/product/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<List<Product>> GetAll() => await _productService.GetAllAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Product>> GetById(string id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<IActionResult> Post(EditProduct newProduct)
    {
        var prd = await _productService.CreateAsync(newProduct);
        return CreatedAtAction(nameof(GetById), new { id = prd.Id }, prd);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, EditProduct updateProduct)
    {
        var product = await _productService.UpdateAsync(id, updateProduct);
        if (product is null)
        {
            return NotFound();
        }
       
        return AcceptedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Remove(string id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        await _productService.RemoveAsync(id);
        return NoContent();
    }

    [HttpPatch("add-to-stock/{productId:length(24)}")]
    public async Task<IActionResult> PatchStock(string productId, AddToStock addToStock)
    {
        var product = await _productService.AddToStock(productId, addToStock);
        if (product is null)
        {
            return NotFound();
        }

        return AcceptedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPatch("remove-from-stock/{productId:length(24)}")]
    public async Task<IActionResult> PatchStock(string productId, RemoveFromStock removeFromStock)
    {
        var product = await _productService.RemoveFromStock(productId, removeFromStock);
        if (product is null)
        {
            return NotFound();
        }

        return AcceptedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

}
