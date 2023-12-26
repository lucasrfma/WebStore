using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductProto;

namespace Products.Models.Entities;

public class ProductDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ShippingPropertiesDb ShippingProperties { get; set; } = null!;
    public List<StockDb>? Stocks { get; set; }

    public static ProductDb FromEditMessage(EditProduct editProduct)
        => new()
        {
            Name = editProduct.Name,
            Description = editProduct.Description,
            ShippingProperties = ShippingPropertiesDb.FromMessage(editProduct.ShippingProperties),
        };

    public Product ToMessage()
    {
        Product message = new()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            ShippingProperties = ShippingProperties.ToMessage()
        };
        if (Stocks is not null) {
            message.Stocks.AddRange(Stocks!.ConvertAll(s => s.ToMessage()));
        }
        return message;
    }
    
    public static ProductDb UpdateProduct(ProductDb productDb, EditProduct editProduct)
    {
        productDb.Name = editProduct.Name;
        productDb.Description = editProduct.Description;
        productDb.ShippingProperties = ShippingPropertiesDb.FromMessage(editProduct.ShippingProperties);
        return productDb;
    }

}
