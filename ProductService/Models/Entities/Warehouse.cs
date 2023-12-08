using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Products.Models.Entities;

public class Warehouse
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = null!;

    public Region AttendedRegions { get; set; }

    public Address Address { get; set; } = null!;
}
