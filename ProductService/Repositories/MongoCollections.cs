using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Products.Config;
using Products.Models.Entities;

namespace Products.Repositories;

public class MongoCollections
{
    public readonly IMongoCollection<ProductDb> ProductsCollection;
    public readonly IMongoCollection<WarehouseDb> WarehousesCollection;
    public readonly IMongoQueryable<ProductDb> ProductsQueryableCollection;
    public readonly IMongoQueryable<WarehouseDb> WarehousesQueryableCollection;

    public MongoCollections(IOptions<ProductDatabaseSettings> productDatabaseSettings)
    {
        var mongoClient = new MongoClient(productDatabaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(productDatabaseSettings.Value.DatabaseName);
        ProductsCollection =
            database.GetCollection<ProductDb>(productDatabaseSettings.Value.ProductsCollectionName);
        WarehousesCollection =
            database.GetCollection<WarehouseDb>(productDatabaseSettings.Value.WarehousesCollectionName);
        ProductsQueryableCollection = ProductsCollection.AsQueryable();
        WarehousesQueryableCollection = WarehousesCollection.AsQueryable();
    }

}
