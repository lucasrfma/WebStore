using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Products.Models.Entities;
using Products.Repositories;
using Products.Models.Dtos;

namespace Products.Services;

public class ProductStorefrontService
{
    private readonly IMongoQueryable<ProductDb> _productsQueryableCollection;
    private readonly IMongoCollection<ProductDb> _productsCollection;
    private readonly IMongoQueryable<WarehouseDb> _warehousesQueryableCollection;

    public ProductStorefrontService(MongoCollections mongoCollections)
    {
        _productsQueryableCollection = mongoCollections.ProductsQueryableCollection;
        _warehousesQueryableCollection = mongoCollections.WarehousesQueryableCollection;
        _productsCollection = mongoCollections.ProductsCollection;
    }

    public async Task<List<ProductListingItem>> ListAvailableProducts()
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
            .Where(pli => pli.AvailableQuantity  > 0)
            .OrderBy(pli => pli.Price)
            .GroupBy(pli => pli.Id)
            .Select(group => group.First());

        return await query.ToListAsync();
    }

    public async Task<List<ProductListingItem>> ListUnavailableProducts()
    {
        var query = _productsQueryableCollection
            .Where(p => p.Stocks != null && !p.Stocks.Exists(s => s.AvailableQuantity > 0))
            .Select(p => new ProductListingItem()
            {
                Id = p.Id!,
                Name = p.Name,
            });
        return await query.ToListAsync();
    }

    public async Task<List<ProductListingItem>> ListProducts()
    {
        List<Task<List<ProductListingItem>>> allProducts = [
            ListAvailableProducts(),
            ListUnavailableProducts()
        ];

        await Task.WhenAll(allProducts);
        return allProducts.SelectMany(t => t.Result).ToList();
    }

    public async Task<List<ProductListingItem>> ListAvailableProducts(Region targetRegion)
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
            .Where(pli => pli.AvailableQuantity > 0)
            .Join(_warehousesQueryableCollection,
                pli => pli.WarehouseId,
                wh => wh.Id,
                (pli, wh) => new { ProductListingItem = pli, Region = wh.AttendedRegions }
            )
            .Where(pli_reg => pli_reg.Region.HasFlag(targetRegion))
            .Select(pli_reg => pli_reg.ProductListingItem)
            .OrderBy(pli=> pli.Price)
            .GroupBy(pli => pli.Id)
            .Select(group => group.First());

        return await query.ToListAsync();
    }


    public async Task<List<ProductListingItem>> ListUnavailableProducts(Region targetRegion)
    {
        var query = _productsQueryableCollection
            .SelectMany(p => p.Stocks, (p, s) => new ProductListingItem()
            {
                Id = p.Id!,
                Name = p.Name,
                AvailableQuantity = s.AvailableQuantity,
                WarehouseId = s.WarehouseId,
            })
           .Join(_warehousesQueryableCollection,
                pli => pli.WarehouseId,
                wh => wh.Id,
                (pli, wh) => new { ProductListingItem = pli, Region = wh.AttendedRegions }
            )
            .Where(pli_reg => pli_reg.Region.HasFlag(targetRegion))
            .Select(pli_reg => pli_reg.ProductListingItem)
            .GroupBy(pli => pli.Id)
            .Select(group => group.Aggregate((pliA,pliB) => new ProductListingItem()
                {
                    Id = pliA.Id!,
                    Name = pliA.Name,
                    AvailableQuantity = pliA.AvailableQuantity + pliB.AvailableQuantity,
                    WarehouseId = null,
                })
            )
            .Where(pli => pli.AvailableQuantity == 0);
        return await query.ToListAsync();
    }

    public async Task<List<ProductListingItem>> ListProducts(Region targetRegion)
    {
        List<Task<List<ProductListingItem>>> allProducts = [
            ListAvailableProducts(targetRegion),
            ListUnavailableProducts(targetRegion)
        ];

        await Task.WhenAll(allProducts);
        return allProducts.SelectMany(t => t.Result).ToList();
    }
}
