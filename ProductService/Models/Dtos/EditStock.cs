using Products.Models.Entities;

namespace Products.Models.Dtos;

/// <summary>
/// DTO to create/edit Stocks. It does not have Quantity and Cost attributes
/// because that sort of information should not be directly set, but calculated
/// when items are added or removed from the Stock.
/// </summary>
public class EditStock
{
    public string StockName { get; set; } = null!;
    public string WarehouseId { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }

    public static EditStock fromStock(Stock stock) => new()
    {
        StockName = stock.StockName,
        WarehouseId = stock.WarehouseId,
        Description = stock.Description,
        Price = stock.Price,
    };

    public static Stock ToStock(EditStock editStockDto) => new()
    {
        StockName = editStockDto.StockName,
        WarehouseId = editStockDto.WarehouseId,
        Description = editStockDto.Description,
        Price = editStockDto.Price,
    };

    public Stock ToStock() => ToStock(this);

    public static Stock UpdateStock(Stock stock, EditStock editStockDto)
    {
        stock.StockName = editStockDto.StockName;
        stock.WarehouseId = editStockDto.WarehouseId;
        stock.Description = editStockDto.Description;
        stock.Price = editStockDto.Price;
        return stock;
    }

    public Stock UpdateStock(Stock stock) => UpdateStock(stock, this);
}
