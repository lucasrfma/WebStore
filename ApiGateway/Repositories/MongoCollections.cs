using ApiGateway.Config;
using ApiGateway.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiGateway.Repositories;

public class MongoCollections
{
    public readonly IMongoCollection<User> UsersCollection;

    public MongoCollections(IOptions<ProductDatabaseSettings> productDatabaseSettings)
    {
        var mongoClient = new MongoClient(productDatabaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(productDatabaseSettings.Value.DatabaseName);
        UsersCollection = database
            .GetCollection<User>(productDatabaseSettings.Value.UserCollectionName);
    }
}