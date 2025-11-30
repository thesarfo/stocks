using System;
using System.Text.RegularExpressions;

namespace Stocks.Backend.Models.Stock;

public class Order
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Symbol { get; set; }
    public OrderSide Side { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public OrderStatus? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum OrderSide
{
    Buy,
    Sell
}


public enum OrderStatus
{
    Placed,
    PartiallyFilled,
    Filled,
    Cancelled
}
