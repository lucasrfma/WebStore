using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Products.Models.Entities;
using Products.Repositories;

namespace Products.Services;

public class WarehouseService
{
    private readonly IMongoCollection<Warehouse> _warehousesCollection;

    public WarehouseService(MongoCollections mongoCollections)
    {
        _warehousesCollection = mongoCollections.warehousesCollection;
    }

    public async Task<List<Warehouse>> GetAllAsync() => 
        await _warehousesCollection.Find(_ => true).ToListAsync();

    public async Task<Warehouse?> GetByIdAsync(string id) => 
        await _warehousesCollection.Find(w =>  w.Id == id).FirstAsync();

    public async Task CreateAsync(Warehouse newWarehouse) =>
        await _warehousesCollection.InsertOneAsync(newWarehouse);

    public async Task UpdateAsync(string id, Warehouse updatedWarehouse) =>
        await _warehousesCollection.ReplaceOneAsync(w =>  w.Id == id, updatedWarehouse);

    public async Task RemoveAsync(string id) =>
        await _warehousesCollection.DeleteOneAsync(w => w.Id == id);
}
