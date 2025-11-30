using System;

namespace Stocks.Backend.Models.Stock;

public class Trade
{
    public string Id { get; set; }
    public string BuyOrderId { get; set; }
    public string SellOrderId { get; set; }
    public string Symbol { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime ExecutedAt { get; set; }
}
