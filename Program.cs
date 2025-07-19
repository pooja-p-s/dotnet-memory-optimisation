using System;

namespace TradingPlatform
{
    class Program
    {
        static void Main(string[] args)
        {

            int size = 10; // Define the size of the order book

            OrderBook orderBook = new OrderBook(size);
            SimularIncomingOrders(orderBook, size);

            User buyer = new User(1, "Alice");
            buyer.Balance = 10000; // Set initial balance
    
            int lowestSellIndex;
            double lowestSellPrice = orderBook.GetLowestSellPrice(out lowestSellIndex);
            Console.WriteLine($"Lowest Sell Price: {lowestSellPrice} at Index: {lowestSellIndex}");

            orderBook.PrintOrders();

            bool purchaseSuccess = orderBook.BuyAtLowestSellPrice(20, 150, buyer);

            orderBook.PrintOrders();    
        }

        static void SimularIncomingOrders(OrderBook orderBook, int size)
        {
            Order[] orders = new Order[size];

            unsafe{
                fixed (Order* ordersPtr = orders)
                {
                    for(int i = 0; i < size; i++)
                    {
                        if(i<size/2){
                            ordersPtr[i] = new Order { Id = i + 1, IsBuyOrder = true, Price = 100 + i, Quantity = 10 + i };
                        }
                        else{
                            // Set sell order prices BELOW or EQUAL to 20 for testing
                            ordersPtr[i] = new Order { Id = i + 1, IsBuyOrder = false, Price = 10 + i, Quantity = 15 + i };
                        }
                        orderBook.AddOrder(ordersPtr[i]);
                    }
                }
            }
        }
    }
}