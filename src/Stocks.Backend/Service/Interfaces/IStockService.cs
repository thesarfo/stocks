using System;
using Stocks.Backend.Models.Stock;

namespace Stocks.Backend.Service.Interfaces;

public interface IStockService
{
    Task<string> PlaceOrder(CreateOrderRequest createOrderRequest);
}
