using System;
using Stocks.Backend.Models.Stock;
using Stocks.Backend.Service.Interfaces;

namespace Stocks.Backend.Service.Providers;

public class StockService : IStockService
{
    public Task<string> PlaceOrder(CreateOrderRequest createOrderRequest)
    {
        // register actor
        // create order
        // let actor fire order to topic
        var order = new Order
        {
            Id = Guid.NewGuid().ToString(),
            UserId = createOrderRequest.UserId,
            Symbol = createOrderRequest.Symbol,
            Side = createOrderRequest.Side,
            Quantity = createOrderRequest.Qty,
            Price = createOrderRequest.Price,
            Status = null, // update this when the order is saved in db
        };

        throw new NotImplementedException();
    }
}
