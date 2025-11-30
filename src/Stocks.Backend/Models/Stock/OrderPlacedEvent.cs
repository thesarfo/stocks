using System;

namespace Stocks.Backend.Models.Stock;

public class OrderPlacedEvent
{
    public string OrderId { get; set; }
    public string ClientId { get; set; }
    public string Symbol { get; set; }
    public OrderSide Side { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Type { get; set; }
    public DateTime Timestamp { get; set; }
    public string IdempotencyKey { get; set; }
}

public class CreateOrderRequest
{
    public string Symbol {get; set;}

    public string UserId {get; set;}
    public OrderSide Side {get; set;}
    public decimal Qty {get; set;}
    public decimal Price {get; set;}
    public string IdempotencyKey {get; set;}

}


public class OrderCancelledEvent
{
    public string OrderId { get; set; }
    public DateTime Timestamp { get; set; }
}


public class OrderStatusChangedEvent
{
    public string OrderId { get; set; }
    public OrderStatus Status { get; set; }
}


public class TradeExecutedEvent
{
    public string TradeId { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime Timestamp { get; set; }
    public string BuyOrderId { get; set; }
    public string SellOrderId { get; set; }
}


public class OrderBook
{
    public string Symbol { get; set; }
    public SortedSet<Order> BuyOrders { get; set; }
    public SortedSet<Order> SellOrders { get; set; }
}


public class BalanceLock
{
    public string UserId { get; set; }
    public decimal LockedAmount { get; set; }
}

