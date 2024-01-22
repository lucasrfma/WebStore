using System.Text;
using System.Text.Json.Serialization;
using ApiGateway.Config;
using ApiGateway.Repositories;
using ApiGateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "WebStoreApp",
            ValidAudience = "WebStoreApp",
        };
        opt.Configuration = new OpenIdConnectConfiguration
        {
            SigningKeys = { new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenService.Key)) }
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddGrpc();
builder.Services.AddSingleton<MongoCollections>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.UseGrpcWeb();

app.MapControllers();

app.UseEndpoints(_ =>
{
    app.MapGrpcService<ProductServiceImpl>()
        .EnableGrpcWeb()
        .RequireCors("AllowLocalhost");
    app.MapGrpcService<LoginServiceImpl>()
        .EnableGrpcWeb()
        .RequireCors("AllowLocalhost");
});

app.Run();
