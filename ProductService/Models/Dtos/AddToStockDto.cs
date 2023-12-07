namespace Products.Models.Dtos
{
    public class AddToStockDto
    {
        public string StockName { get; set; } = null!;
        public ulong Quantity { get; set; }
        public decimal Cost { get; set; }
    }
}
