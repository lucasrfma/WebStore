using System.Text.Json.Serialization;
using ApiGateway.Services;

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

builder.Services.AddGrpc();

builder.Services.AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRouting();
app.UseCors();
app.UseGrpcWeb();

app.MapControllers();

app.UseEndpoints(_ =>
{
    app.MapGrpcService<ProductServiceImpl>()
        .EnableGrpcWeb()
        .RequireCors("AllowLocalhost");
});

app.Run();
