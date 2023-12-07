﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Products.Models.Entities;

public class Stock
{
    public string StockName { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
    public ulong AvailableQuantity { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
}