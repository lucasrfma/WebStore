using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Products.Models.Entities;
using Products.Repositories;

namespace Products.Services;

public class WarehouseService
{
    private readonly IMongoCollection<Warehouse> warehousesCollection;
    private readonly IMongoQueryable<Warehouse> warehousesQueryableCollection;

    public WarehouseService(QueryableCollections queryableCollections)
    {
        warehousesCollection = queryableCollections.warehousesCollection;
        warehousesQueryableCollection = queryableCollections.warehousesQueryableCollection;
    }

    public async Task<List<Warehouse>> GetAllAsync() => 
        await warehousesCollection.Find(_ => true).ToListAsync();

    public async Task<Warehouse?> GetByIdAsync(string id) => 
        await warehousesCollection.Find(w =>  w.Id == id).FirstAsync();

    public async Task CreateAsync(Warehouse newWarehouse) =>
        await warehousesCollection.InsertOneAsync(newWarehouse);

    public async Task UpdateAsync(string id, Warehouse updatedWarehouse) =>
        await warehousesCollection.ReplaceOneAsync(w =>  w.Id == id, updatedWarehouse);

    public async Task RemoveAsync(string id) =>
        await warehousesCollection.DeleteOneAsync(w => w.Id == id);
}
