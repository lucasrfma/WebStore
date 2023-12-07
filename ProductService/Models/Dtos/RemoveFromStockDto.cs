namespace Products.Models.Dtos
{
    public class RemoveFromStockDto
    {
        public string StockName { get; set; } = null!;
        public ulong Quantity { get; set; }
    }
}
