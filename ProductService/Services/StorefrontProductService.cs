using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Products.Models.Entities;
using Products.Repositories;
using Products.Models.Dtos;

namespace Products.Services;

public class StorefrontProductService
{
    private readonly IMongoQueryable<Product> _productsQueryableCollection;
    private readonly IMongoQueryable<Warehouse> _warehousesQueryableCollection;

    public StorefrontProductService(QueryableCollections queryableCollections)
    {
        _productsQueryableCollection = queryableCollections.productsQueryableCollection;
        _warehousesQueryableCollection = queryableCollections.warehousesQueryableCollection;
    }

    public async Task<List<ProductListingItem>> ListProducts()
    {
        var query = _productsQueryableCollection
            .SelectMany(p => p.Stocks, (p, s) => new ProductListingItem()
            {
                Id = p.Id!,
                Name = p.Name,
                StockDescription = s.Description,
                Price = s.Price,
                AvailableQuantity = s.AvailableQuantity,
                WarehouseId = s.WarehouseId,
            })
            .OrderBy(pli => pli.Price)
            .GroupBy(pli => pli.Id)
            .Select(group => group.First());

        return await query.ToListAsync();
    }

    public async Task<List<ProductListingItem>> ListProducts(Region targetRegion)
    {
        var query = _productsQueryableCollection
            .SelectMany(p => p.Stocks, (p, s) => new ProductListingItem()
            {
                Id = p.Id!,
                Name = p.Name,
                StockDescription = s.Description,
                Price = s.Price,
                AvailableQuantity = s.AvailableQuantity,
                WarehouseId = s.WarehouseId,
            }).Join(_warehousesQueryableCollection,
                pli => pli.WarehouseId,
                wh => wh.Id,
                (pli, wh) => new { ProductListingItem = pli, Region = wh.AttendedRegions }
            ).Where(pli_reg => pli_reg.Region.HasFlag(targetRegion))
            .Select(pli_reg => pli_reg.ProductListingItem)
            .OrderBy(pli=> pli.Price)
            .GroupBy(pli => pli.Id)
            .Select(group => group.First());

        return await query.ToListAsync();
    }
}
