namespace Products.Models.Dtos;

/// <summary>
/// Simplified Product response to be used when listing items on the front end.
/// </summary>
public class ProductListingItem
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    // this attribute can be used to show a special tag on the product listing
    // to let the costumer know it is a limited sale or something similar.
    public string? StockDescription { get; set; }
    public decimal? Price { get; set; }
    public ulong? AvailableQuantity { get; set; }
    public string? WarehouseId { get; set; }
}
