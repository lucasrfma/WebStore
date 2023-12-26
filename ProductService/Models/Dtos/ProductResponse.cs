using Products.Models.Entities;
using ProductProto;

namespace Products.Models.Dtos;

/// <summary>
/// Product response with all details that matter for a customer.
/// It could be used in more places, but the main target for this DTO is
/// a product details page in the Front End.<br></br>
/// <br></br>
/// For that end, this would be used to generate a response for a single product,
/// with the Stocks filtered to only show stocks that have the customer's Region
/// in it's AttendedRegions enum, sorted by Price ascending.<br></br>
/// <br></br>
/// This way, the details page would mainly use the first element of the array. 
/// Why not make it a single StockResponseDto, instead of an array then?
/// Simply so that in case a stock is near its end, the customer can check how much
/// it will cost if he takes too long to buy and the cheapest option ends.<br></br>
/// <br></br>
/// (There is also the possibility for a stock located in a closer warehouse to have
/// a higher price tag, but the overall cost for the customer be lower due to cheaper
/// shipping. So customers with enough patience could check all possibilities. 
/// That or eventually add functionality to the system to check shipping for all options
/// returned by this DTO and sort by the total of Price+Shipping)
/// </summary>
public class ProductResponse
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ShippingProperties ShippingProperties { get; set; } = null!;
    public List<StockResponse>? Stocks { get; set; }


    public class StockResponse
    {
        //public string WarehouseId { get; set; } = null!;
        //public string StockName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ulong AvailableQuantity { get; set; }
        public AddressDb AddressDb { get; set; } = null!;
    }
}
