namespace TradingPlatform
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    //unsafe is added with all functions for readability 
    //Explaining usage of NativeMemory and unsafe code in C#.

    public unsafe class OrderBook
    {
        const int size = 10;

        Order* buyOrders;
        Order* sellOrders;

        public delegate void PriceNotificationHandler(object sender, PriceNotificationEventArgs e);
 
        public event PriceNotificationHandler? PriceNotification;

        private double highestBuyPrice;
        private double lowestSellPrice;

        public OrderBook()
        {
            buyOrders = (Order*)NativeMemory.Alloc((nuint)size, (nuint)sizeof(Order));
            sellOrders = (Order*)NativeMemory.Alloc((nuint)size, (nuint)sizeof(Order));
            
            for (int i = 0; i < size; i++)
            { 
                buyOrders[i] = new Order(){Id = 0};
                sellOrders[i] = new Order(){Id = 0};
            }

            highestBuyPrice = double.MinValue;
            lowestSellPrice = double.MaxValue;  
        }

        protected virtual void OnPriceNotification(PriceNotificationEventArgs e)
        {
            PriceNotification?.Invoke(this, e);
        }

        ~OrderBook()
        {
            NativeMemory.Free(buyOrders);
            NativeMemory.Free(sellOrders);
        } 

        public unsafe void AddOrder(Order newOrder)
        {
            Order* targetOrders = newOrder.IsBuyOrder ? buyOrders : sellOrders;
            for (int i = 0; i < size; i++){
                if (targetOrders[i].Id == 0) // Assuming Id 0 means empty slot
                {
                    targetOrders[i] = newOrder;
                    UpdateAndNotify();
                    return;
                }
            }
        }

        public unsafe void RemoveOrder(int orderId){
            Order* targetOrders = null;
            int orderIndex = -1;
            for (int i = 0; i < size; i++)
            {
                if (buyOrders[i].Id == orderId)
                {
                    targetOrders = buyOrders;
                    orderIndex = -1;
                    break;
                }
                else if (sellOrders[i].Id == orderId)
                {
                    targetOrders = sellOrders;
                    orderIndex = -1;
                    break;
                }
            }
            if (targetOrders != null)
            {
                targetOrders[orderIndex] = new Order(); // Reset the order
            }
        }

        public unsafe void PrintOrders()
        {
            Console.WriteLine("Buy Orders:");
            for (int i = 0; i < size; i++)
            {
                if (buyOrders[i].Id != 0) // Assuming Id 0 means empty slot
                {
                    Console.WriteLine($"Id={buyOrders[i].Id}, Price={buyOrders[i].Price}, Quantity={buyOrders[i].Quantity}, IsBuyOrder={buyOrders[i].IsBuyOrder}");
                }
            }

            Console.WriteLine("Sell Orders:");
            for (int i = 0; i < size; i++)
            {
                if (sellOrders[i].Id != 0) // Assuming Id 0 means empty slot
                {
                    Console.WriteLine($"Id={sellOrders[i].Id}, Price={sellOrders[i].Price}, Quantity={sellOrders[i].Quantity}, IsBuyOrder={sellOrders[i].IsBuyOrder}");
                }
            }
        }
        private unsafe void UpdateAndNotify()
        {

            fixed(double * fixedHighestBuyPrice = &highestBuyPrice, 
                  fixedLowestSellPrice = &lowestSellPrice)
            {
                *fixedHighestBuyPrice = double.MinValue;
                *fixedLowestSellPrice = double.MaxValue;

                for (int i = 0; i < size; i++)
                {
                    if (buyOrders[i].Id != 0)
                    {
                        if (buyOrders[i].Price > *fixedHighestBuyPrice)
                        {
                            *fixedHighestBuyPrice = buyOrders[i].Price;
                            OnPriceNotification(new PriceNotificationEventArgs(*fixedHighestBuyPrice, true));
                        }
                    }

                    if (sellOrders[i].Id != 0)
                    {
                        if (sellOrders[i].Price < *fixedLowestSellPrice)
                        {
                            *fixedLowestSellPrice = sellOrders[i].Price;
                            OnPriceNotification(new PriceNotificationEventArgs(*fixedLowestSellPrice, false));
                        }
                    }
                }    
            }
            
        }
    }
}