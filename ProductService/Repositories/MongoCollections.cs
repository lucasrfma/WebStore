using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Products.Config;
using Products.Models.Entities;

namespace Products.Repositories;

public class MongoCollections
{
    public readonly IMongoCollection<ProductDb> productsCollection;
    public readonly IMongoCollection<WarehouseDb> warehousesCollection;
    public readonly IMongoQueryable<ProductDb> productsQueryableCollection;
    public readonly IMongoQueryable<WarehouseDb> warehousesQueryableCollection;

    public MongoCollections(IOptions<ProductDatabaseSettings> productDatabaseSettings)
    {
        var mongoClient = new MongoClient(productDatabaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(productDatabaseSettings.Value.DatabaseName);
        productsCollection =
            database.GetCollection<ProductDb>(productDatabaseSettings.Value.ProductsCollectionName);
        warehousesCollection =
            database.GetCollection<WarehouseDb>(productDatabaseSettings.Value.WarehousesCollectionName);
        productsQueryableCollection = productsCollection.AsQueryable();
        warehousesQueryableCollection = warehousesCollection.AsQueryable();
    }

}
