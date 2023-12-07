using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Products.Config;
using Products.Models.Entities;

namespace Products.Repositories;

public class QueryableCollections
{
    public readonly IMongoCollection<Product> productsCollection;
    public readonly IMongoCollection<Warehouse> warehousesCollection;
    public readonly IMongoQueryable<Product> productsQueryableCollection;
    public readonly IMongoQueryable<Warehouse> warehousesQueryableCollection;

    public QueryableCollections(IOptions<ProductDatabaseSettings> productDatabaseSettings)
    {
        var mongoClient = new MongoClient(productDatabaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(productDatabaseSettings.Value.DatabaseName);
        productsCollection =
            database.GetCollection<Product>(productDatabaseSettings.Value.ProductsCollectionName);
        warehousesCollection =
            database.GetCollection<Warehouse>(productDatabaseSettings.Value.WarehousesCollectionName);
        productsQueryableCollection = productsCollection.AsQueryable();
        warehousesQueryableCollection = warehousesCollection.AsQueryable();
    }

}
