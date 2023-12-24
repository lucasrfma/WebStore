using CommonProto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Products.Models.Dtos;
using Products.Models.Entities;
using Products.Repositories;
using ProductProto;
using static ProductProto.ProductService;

namespace Products.Services;

public class ProductServiceImpl(MongoCollections mongoCollections) : ProductServiceBase
{
    private readonly IMongoCollection<ProductDb> _productsCollection = mongoCollections.productsCollection;

    private readonly IMongoQueryable<ProductDb> _productsQueryableCollection =
        mongoCollections.productsQueryableCollection;

    public override async Task<ProductList> GetAll(Empty request, ServerCallContext context)
    {
        ProductList pl = new ProductList();
        var products = await _productsCollection.Find(_ => true)
            .ToListAsync();
        pl.Products.AddRange(products.ConvertAll(p=> p.ToMessage()));
        return pl;
    }

    public override async Task<Product> GetById(MongoId request, ServerCallContext context)
    {
        ProductDb product = await _productsQueryableCollection
            .FirstAsync(p => p.Id == request.Id);
        return product.ToMessage();
    }

    public async Task<Product> CreateAsync(EditProduct newProduct)
    {
        ProductDb prd = ProductDb.FromEditMessage(newProduct);
        await _productsCollection.InsertOneAsync(prd);
        return prd.ToMessage();
    }

    public async Task<Product?> UpdateAsync(string id, EditProduct updateProduct)
    {
        ProductDb product = await _productsQueryableCollection
            .FirstAsync(p => p.Id == id);
        if (product == null)
        {
            return null;
        }

        await _productsCollection.ReplaceOneAsync(p => p.Id == id,
            ProductDb.UpdateProduct(product, updateProduct));
        return product.ToMessage();
    }

    public async Task<Product?> AddToStock(string id, AddToStock addToStock)
    {
        ProductDb product = await _productsQueryableCollection
            .FirstAsync(p => p.Id == id);
        StockDb? stock = product?.Stocks?
            .Where(s => s.StockName == addToStock.StockName).FirstOrDefault();
        bool wasAdded = stock?.AddToStock(addToStock.Quantity, (decimal) addToStock.Cost) ?? false;
        
        if (product is null || stock is null || !wasAdded)
        {
            return null;
        }

        var update = Builders<ProductDb>.Update
            .Set(p => p.Stocks, product.Stocks);
        await _productsCollection.UpdateOneAsync(p => p.Id == id, update);

        return product.ToMessage();
    }

    public async Task<Product?> RemoveFromStock(string id, RemoveFromStock removeFromStock)
    {
        ProductDb product = await _productsQueryableCollection
            .FirstAsync(p => p.Id == id);
        StockDb? stock = product?.Stocks?
            .Where(s => s.StockName == removeFromStock.StockName).FirstOrDefault();
        bool wasRemoved = stock?.RemoveFromStock(removeFromStock.Quantity) ?? false;

        if (product is null || stock is null || !wasRemoved)
        {
            return null;
        }

        var update = Builders<ProductDb>.Update
            .Set(p => p.Stocks, product.Stocks);
        await _productsCollection.UpdateOneAsync(p => p.Id == id, update);

        return product.ToMessage();
    }

    public async Task RemoveAsync(string id) =>
        await _productsCollection.DeleteOneAsync(p => p.Id == id);
}
