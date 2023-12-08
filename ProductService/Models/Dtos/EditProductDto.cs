using Products.Models.Entities;

namespace Products.Models.Dtos;

public class EditProductDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ShippingProperties ShippingProperties { get; set; } = null!;
    public List<EditStockDto>? EditStockDtos { get; set; }

    public static EditProductDto FromProduct(Product product)
    {
        return new EditProductDto()
        {
            Name = product.Name,
            Description = product.Description,
            ShippingProperties = product.ShippingProperties,
            EditStockDtos = product.Stocks?.Select(EditStockDto.fromStock).ToList()
        };
    }


    public static Product ToProduct(EditProductDto editProductDto)
    {;
        return new Product()
        {
            Name = editProductDto.Name,
            Description = editProductDto.Description,
            ShippingProperties = editProductDto.ShippingProperties,
            Stocks = editProductDto.EditStockDtos?.Select(EditStockDto.ToStock).ToList()
        };
    }

    public Product ToProduct() => ToProduct(this);

    public static Product EditProduct(Product product, EditProductDto editProductDto)
    {
        product.Name = editProductDto.Name;
        product.Description = editProductDto.Description;
        product.ShippingProperties = editProductDto.ShippingProperties;
        product.Stocks = editProductDto.EditStockDtos?.Select(EditStockDto.ToStock).ToList();
        return product;
    }

    public Product EditProduct(Product product) => EditProduct(product, this);
}
