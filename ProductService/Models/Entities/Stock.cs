﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Products.Models.Entities;

public class Stock
{
    public string StockName { get; set; } = null!;
    [BsonRepresentation(BsonType.ObjectId)]
    public string WarehouseId { get; set; } = null!;
    public ulong AvailableQuantity { get; set; }
    // this attribute should be used to describe special stock
    // eg: BLACK FRIDAY!! Only first 20 orders!!
    public string? Description { get; set; }

    private decimal _price;
    public decimal Price { get { return _price;  } set { _price = Math.Round(value, 2); } }
    private decimal _cost;
    public decimal Cost { get { return _cost; } set { _cost = Math.Round(value, 2); } }

    /// <summary>
    /// Adds newly bought items to this stock and recalculates this Stocks cost (Average unit cost).
    /// Doesn't alter any properties if new item's buying cost is less than 0.<br></br>
    /// PS: Cost here is always unit cost
    /// </summary>
    /// <param name="addedQuantity"></param>
    /// <param name="newBuyingCost"></param>
    /// <returns>boolean representing if addition was accepted or not.</returns>
    public bool AddToStock(ulong addedQuantity, decimal newBuyingCost)
    {
        if (newBuyingCost < 0) return false;

        var previousQuantity = AvailableQuantity;
        var newQuantity = previousQuantity + addedQuantity;
        var previousCost = Cost;

        var newCost = ((previousQuantity * previousCost) + (newBuyingCost * addedQuantity))
                        / newQuantity;

        Cost = newCost;
        AvailableQuantity = newQuantity;
        return true;
    }

    /// <summary>
    /// Removes items from stock. Rejects removal if removedQuantity is greater than available quantity.
    /// </summary>
    /// <param name="removedQuantity"></param>
    /// <returns>boolean representing if removal was accepted or not.</returns>
    public bool RemoveFromStock(ulong removedQuantity)
    {
        if(AvailableQuantity < removedQuantity) return false;
        AvailableQuantity -= removedQuantity;
        return true;
    }
}
