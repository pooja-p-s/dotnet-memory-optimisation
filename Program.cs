using System;

namespace TradingPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            User user1 = new User(1, "Alice");

            user1.SubscribeToBuyTargetPrice(99.0);
            user1.SubscribeToBuyTargetPrice(100.0);
            user1.SubscribeToSellTargetPrice(105.0);
            user1.SubscribeToSellTargetPrice(101.0);

            OrderBook orderBook = new OrderBook();
            orderBook.PriceNotification += user1.OnPriceNotification;

            Order newBuyOrder = new Order { Id = 1, IsBuyOrder = true, Price = 101.5, Quantity = 100 };
            orderBook.AddOrder(newBuyOrder);

            Order newSellOrder = new Order { Id = 2, IsBuyOrder = false, Price = 102.5, Quantity = 50 };
            orderBook.AddOrder(newSellOrder);

            orderBook.PrintOrders();
        }
    }

}