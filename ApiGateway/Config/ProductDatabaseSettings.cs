namespace ApiGateway.Config;

public class ProductDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set;} = null!;
    public string UserCollectionName { get; set; } = null!;
}