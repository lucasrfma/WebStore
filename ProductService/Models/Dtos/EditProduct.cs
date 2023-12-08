using Products.Models.Entities;

namespace Products.Models.Dtos;

/// <summary>
/// DTO to create/edit products. It uses EditStock DTOs to avoid direct setting of
/// quantity and cost.
/// </summary>
public class EditProduct
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ShippingProperties ShippingProperties { get; set; } = null!;
    public List<EditStock>? EditStockDtos { get; set; }

    public static EditProduct FromProduct(Product product)
    {
        return new EditProduct()
        {
            Name = product.Name,
            Description = product.Description,
            ShippingProperties = product.ShippingProperties,
            EditStockDtos = product.Stocks?.Select(EditStock.fromStock).ToList()
        };
    }


    public static Product ToProduct(EditProduct editProductDto)
    {;
        return new Product()
        {
            Name = editProductDto.Name,
            Description = editProductDto.Description,
            ShippingProperties = editProductDto.ShippingProperties,
            Stocks = editProductDto.EditStockDtos?.Select(EditStock.ToStock).ToList()
        };
    }

    public Product ToProduct() => ToProduct(this);

    public static Product UpdateProduct(Product product, EditProduct editProductDto)
    {
        product.Name = editProductDto.Name;
        product.Description = editProductDto.Description;
        product.ShippingProperties = editProductDto.ShippingProperties;
        product.Stocks = editProductDto.EditStockDtos?.Select(EditStock.ToStock).ToList();
        return product;
    }

    public Product UpdateProduct(Product product) => UpdateProduct(product, this);
}
