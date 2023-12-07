namespace Products.Config;

public class ProductDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set;} = null!;
    public string ProductsCollectionName { get; set; } = null!;
    public string WarehousesCollectionName { get; set; } = null!;
}
