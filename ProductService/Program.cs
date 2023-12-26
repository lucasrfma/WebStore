using Products.Config;
using Products.Repositories;
using Products.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowLocalhost", p =>
    {
        p.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:5106");
    });
});

builder.Services.Configure<ProductDatabaseSettings>(
    builder.Configuration.GetSection("ProductDatabase"));

builder.Services.AddSingleton<MongoCollections>();
builder.Services.AddSingleton<WarehouseService>();
// builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<StorefrontProductService>();

builder.Services.AddGrpc();

builder.Services.AddControllers()
    .AddJsonOptions(options => 
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors();
app.UseGrpcWeb();

app.UseAuthorization();

app.MapControllers();
// app.MapGrpcService<ProductServiceImpl>();
app.UseEndpoints(e =>
{
    app.MapGrpcService<ProductServiceImpl>()
        .EnableGrpcWeb()
        .RequireCors("AllowLocalhost");
});

app.Run();
