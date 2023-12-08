using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Products.Models.Entities;

public class Stock
{
    public string StockName { get; set; } = null!;
    [BsonRepresentation(BsonType.ObjectId)]
    public string WarehouseId { get; set; } = null!;
    public ulong AvailableQuantity { get; set; }
    // this attribute should be used to describe special stock
    // eg: BLACK FRIDAY!! Only first 20 orders!!
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
}
