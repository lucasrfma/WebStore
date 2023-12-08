namespace Products.Models.Dtos;

public class RemoveFromStock
{
    public string StockName { get; set; } = null!;
    public ulong Quantity { get; set; }
}
