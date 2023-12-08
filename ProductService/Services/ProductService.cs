using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Products.Models.Entities;
using Products.Repositories;

namespace Products.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductService(QueryableCollections queryableCollections)
    {
        _productsCollection = queryableCollections.productsCollection;
    }

    public async Task<List<Product>> GetAllAsync() =>
    await _productsCollection.Find(_ => true).ToListAsync();

    public async Task<Product?> GetByIdAsync(string id) =>
        await _productsCollection.Find(w => w.Id == id).FirstAsync();

    public async Task CreateAsync(Product newProduct) =>
        await _productsCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(p => p.Id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await _productsCollection.DeleteOneAsync(p => p.Id == id);
}
