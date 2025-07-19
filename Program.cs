using System;

namespace TradingPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderBook orderBook = new OrderBook(100);

            SimulateIncomingOrders(orderBook, 10);

            OrderCancellationRequest[] cancellations = new OrderCancellationRequest[]
            {
                new OrderCancellationRequest { OrderId = 1 },
                new OrderCancellationRequest { OrderId = 5 },
                new OrderCancellationRequest { OrderId = 7 }
            };

            orderBook.PrintOrders();

            orderBook.BulkCancelOrders(cancellations);

            orderBook.PrintOrders();
        }
        static void SimulateIncomingOrders(OrderBook orderBook, int size)
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