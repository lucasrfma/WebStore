namespace Products.Models.Entities;

public class AddressDb
{
    public Region Region { get; set; }
    public string PostalCode { get; set; } = null!;
    public string Street { get; set; } = null!;
    public uint StreetNumber { get; set; }
}
