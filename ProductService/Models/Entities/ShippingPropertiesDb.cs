using ProductProto;

namespace Products.Models.Entities;

public class ShippingPropertiesDb
{
    public decimal Length { get; set; }
    public decimal Height { get; set; }
    public decimal Width { get; set; }
    public decimal Weight { get; set; }

    public static ShippingPropertiesDb FromMessage(ShippingProperties shippingProperties)
        => new()
        {
            Length = (decimal)shippingProperties.Length,
            Height = (decimal)shippingProperties.Height,
            Width = (decimal)shippingProperties.Width,
            Weight = (decimal)shippingProperties.Weight,
        };

    public ShippingProperties ToMessage() => new()
    {
        Length = (double)Length,
        Height = (double)Height,
        Width = (double)Width,
        Weight = (double)Weight,
    };
}
