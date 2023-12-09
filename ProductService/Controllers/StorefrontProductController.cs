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
    [HttpGet("available")]
    public async Task<List<ProductListingItem>> ListAvailableProducts() => await _sfProductService.ListAvailableProducts();

    [HttpGet("available/{region}")]
    public async Task<List<ProductListingItem>> ListAvailableProducts(Region region) => await _sfProductService.ListAvailableProducts(region);
    [HttpGet("unavailable")]
    public async Task<List<ProductListingItem>> ListUnavailableProducts() => await _sfProductService.ListUnavailableProducts();

    [HttpGet("unavailable/{region}")]
    public async Task<List<ProductListingItem>> ListUnavailableProducts(Region region) => await _sfProductService.ListUnavailableProducts(region);
}
