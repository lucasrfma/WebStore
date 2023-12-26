using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Products.Models.Entities;
using Products.Repositories;

namespace Products.Services;

public class WarehouseService
{
    private readonly IMongoCollection<WarehouseDb> _warehousesCollection;

    public WarehouseService(MongoCollections mongoCollections)
    {
        _warehousesCollection = mongoCollections.warehousesCollection;
    }

    public async Task<List<WarehouseDb>> GetAllAsync() => 
        await _warehousesCollection.Find(_ => true).ToListAsync();

    public async Task<WarehouseDb?> GetByIdAsync(string id) => 
        await _warehousesCollection.Find(w =>  w.Id == id).FirstAsync();

    public async Task CreateAsync(WarehouseDb newWarehouseDb) =>
        await _warehousesCollection.InsertOneAsync(newWarehouseDb);

    public async Task UpdateAsync(string id, WarehouseDb updatedWarehouseDb) =>
        await _warehousesCollection.ReplaceOneAsync(w =>  w.Id == id, updatedWarehouseDb);

    public async Task RemoveAsync(string id) =>
        await _warehousesCollection.DeleteOneAsync(w => w.Id == id);
}
