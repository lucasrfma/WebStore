using Microsoft.AspNetCore.Mvc;
using Products.Models.Dtos;
using Products.Models.Entities;
using Products.Services;

namespace Products.Controllers;

[ApiController]
[Route("product/api/v1/[controller]")]
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
    public async Task<IActionResult> Post(EditProductDto newProduct)
    {
        var prd = EditProductDto.ToProduct(newProduct);
        await _productService.CreateAsync(prd);
        return CreatedAtAction(nameof(GetById), new { id = prd.Id }, newProduct);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Product updatedProduct)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        updatedProduct.Id = product.Id;
        await _productService.UpdateAsync(id, updatedProduct);

        return AcceptedAtAction(nameof(GetById), new { id = updatedProduct.Id }, updatedProduct);
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
}
