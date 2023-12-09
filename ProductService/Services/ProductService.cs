using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Products.Models.Dtos;
using Products.Models.Entities;
using Products.Repositories;

namespace Products.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductService(MongoCollections mongoCollections)
    {
        _productsCollection = mongoCollections.productsCollection;
    }

    public async Task<List<Product>> GetAllAsync() =>
    await _productsCollection.Find(_ => true).ToListAsync();

    public async Task<Product?> GetByIdAsync(string id) =>
        await _productsCollection.Find(w => w.Id == id).FirstAsync();

    public async Task<Product> CreateAsync(EditProduct newProduct)
    {
        var prd = EditProduct.ToProduct(newProduct);
        await _productsCollection.InsertOneAsync(prd);
        return prd;
    }

    public async Task<Product?> UpdateAsync(string id, EditProduct updateProduct)
    {
        var product = await GetByIdAsync(id);
        if (product == null)
        {
            return null;
        }
        await _productsCollection.ReplaceOneAsync(p => p.Id == id,
            EditProduct.UpdateProduct(product, updateProduct));
        return product;
    }

    public async Task<Product?> AddToStock(string id, AddToStock addToStock)
    {
        var product = await GetByIdAsync(id);
        var stock = product?.Stocks?
            .Where(s => s.StockName == addToStock.StockName).FirstOrDefault();
        var wasAdded = stock?.AddToStock(addToStock.Quantity, addToStock.Cost) ?? false;
        
        if (product is null || stock is null || !wasAdded)
        {
            return null;
        }

        var update = Builders<Product>.Update
            .Set(p => p.Stocks, product.Stocks);
        await _productsCollection.UpdateOneAsync(p => p.Id == id, update);

        return product;
    }

    public async Task<Product?> RemoveFromStock(string id, RemoveFromStock removeFromStock)
    {
        var product = await GetByIdAsync(id);
        var stock = product?.Stocks?
            .Where(s => s.StockName == removeFromStock.StockName).FirstOrDefault();
        var wasRemoved = stock?.RemoveFromStock(removeFromStock.Quantity) ?? false;

        if (product is null || stock is null || !wasRemoved)
        {
            return null;
        }

        var update = Builders<Product>.Update
            .Set(p => p.Stocks, product.Stocks);
        await _productsCollection.UpdateOneAsync(p => p.Id == id, update);

        return product;
    }

    public async Task RemoveAsync(string id) =>
        await _productsCollection.DeleteOneAsync(p => p.Id == id);
}
