namespace Products.Models.Dtos;

public class AddToStock
{
    public string StockName { get; set; } = null!;
    public ulong Quantity { get; set; }
    public decimal Cost { get; set; }
}
