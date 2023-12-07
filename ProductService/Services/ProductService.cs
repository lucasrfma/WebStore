using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Products.Config;
using Products.Models.Entities;
using Products.Repositories;

namespace Products.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _productCollection;
    
    public ProductService(QueryableCollections pd, IOptions<ProductDatabaseSettings> settings)
    {
        _productCollection = pd.database.GetCollection<Product>(settings.Value.ProductsCollectionName);
    }

    public async Task<List<Product>> GetAllAsync() =>
    await _productCollection.Find(_ => true).ToListAsync();

    public async Task<Product?> GetByIdAsync(string id) =>
        await _productCollection.Find(w => w.Id == id).FirstAsync();

    public async Task CreateAsync(Product newProduct) =>
        await _productCollection.InsertOneAsync(newProduct);

    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await _productCollection.ReplaceOneAsync(p => p.Id == id, updatedProduct);

    public async Task RemoveAsync(string id) =>
        await _productCollection.DeleteOneAsync(p => p.Id == id);
}
