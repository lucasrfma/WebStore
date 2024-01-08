using Products.Config;
using Products.Repositories;
using Products.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ProductDatabaseSettings>(
    builder.Configuration.GetSection("ProductDatabase"));

builder.Services.AddSingleton<MongoCollections>();
builder.Services.AddSingleton<WarehouseService>();
builder.Services.AddSingleton<ProductStorefrontService>();

builder.Services.AddGrpc();

builder.Services.AddControllers()
    .AddJsonOptions(options => 
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<ProductServiceImpl>();

app.Run();
