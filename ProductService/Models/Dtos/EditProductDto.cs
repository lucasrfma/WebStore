using Products.Models.Entities;

namespace Products.Models.Dtos;

public class EditProductDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ShippingProperties ShippingProperties { get; set; } = null!;

    public static EditProductDto FromProduct(Product product) => new()
    {
        Name = product.Name,
        Description = product.Description,
        ShippingProperties = product.ShippingProperties,
    };

    public static Product ToProduct(EditProductDto editProductDto) => new()
    {
        Name = editProductDto.Name,
        Description = editProductDto.Description,
        ShippingProperties = editProductDto.ShippingProperties,
    };

    public Product ToProduct() => ToProduct(this);

    public static Product EditProduct(Product product, EditProductDto editProductDto) 
    {
        product.Name = editProductDto.Name;
        product.Description = editProductDto.Description;
        product.ShippingProperties = editProductDto.ShippingProperties;
        return product;
    }

    public Product EditProduct(Product product) => EditProduct(product, this);
}
