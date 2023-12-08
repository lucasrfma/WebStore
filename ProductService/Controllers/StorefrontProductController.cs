using Microsoft.AspNetCore.Mvc;
using Products.Models.Dtos;
using Products.Models.Entities;
using Products.Services;

namespace Products.Controllers;

[ApiController]
[Route("storefront/api/v1/product/[controller]")]
public class StorefrontProductController
{
    private readonly StorefrontProductService _sfProductService;

    public StorefrontProductController(StorefrontProductService sfProductService)
    {
        _sfProductService = sfProductService;
    }

    [HttpGet]
    public async Task<List<ProductListingItem>> ListProducts() => await _sfProductService.ListProducts();

    [HttpGet("{region}")]
    public async Task<List<ProductListingItem>> ListProducts(Region region) => await _sfProductService.ListProducts(region);
}
