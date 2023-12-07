using MongoDB.Bson.Serialization.Attributes;

namespace Products.Models.Entities;

public class Address
{
    public Region Region { get; set; }
    public string PostalCode { get; set; } = null!;
    public string Street { get; set; } = null!;
    public nuint StreetNumber { get; set; }
}
