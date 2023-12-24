using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Products.Models.Entities;

public class WarehouseDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = null!;

    public Region AttendedRegions { get; set; }

    public AddressDb AddressDb { get; set; } = null!;
}
