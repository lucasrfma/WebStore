using Products.Models.Entities;

namespace Products.Models.Dtos
{
    public class EditStockDto
    {
        public string StockName { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public static EditStockDto fromStock(Stock stock) => new()
        {
            StockName = stock.StockName,
            Warehouse = stock.Warehouse,
            Description = stock.Description,
            Price = stock.Price,
        };

        public static Stock ToStock(EditStockDto editStockDto) => new()
        {
            StockName = editStockDto.StockName,
            Warehouse = editStockDto.Warehouse,
            Description = editStockDto.Description,
            Price = editStockDto.Price,
        };

        public Stock ToStock() => ToStock(this);

        public static Stock EditStock(Stock stock, EditStockDto editStockDto)
        {
            stock.StockName = editStockDto.StockName;
            stock.Warehouse = editStockDto.Warehouse;
            stock.Description = editStockDto.Description;
            stock.Price = editStockDto.Price;
            return stock;
        }

        public Stock EditStock(Stock stock) => EditStock(stock, this);
    }
}
