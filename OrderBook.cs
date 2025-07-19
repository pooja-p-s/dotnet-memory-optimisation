namespace TradingPlatform
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public unsafe class OrderBook
    {
        private int size;

        Order* buyOrders;
        Order* sellOrders;

        public delegate void PriceNotificationHandler(object sender, PriceNotificationEventArgs e);
        public event PriceNotificationHandler? PriceNotification;

        private double highestBuyPrice;
        private double lowestSellPrice;

        public OrderBook(int size)
        {
            this.size = size;
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
        
        public unsafe double GetLowestSellPrice(out int lowestSellIndex)
        {
            lowestSellIndex = -1;
            double lowestSellPrice = double.MaxValue;
            for (int i = 0; i < size; i++)
            {
                if (sellOrders[i].Id != 0 && sellOrders[i].Price < lowestSellPrice)
                {
                    lowestSellPrice = sellOrders[i].Price;
                    lowestSellIndex = i;        
                }
            }
            return lowestSellPrice == double.MaxValue ? -1 : lowestSellPrice;
        }   

        public unsafe bool BuyAtLowestSellPrice(double maxPrice, int buyQuantity, User buyer)
        {
            int remainingQuantity = buyQuantity;
            double totalCost = 0;

            while (remainingQuantity > 0)
            {
                int lowestSellIndex;
                double lowestSellPrice = GetLowestSellPrice(out lowestSellIndex);

                if (lowestSellPrice > maxPrice || lowestSellIndex == -1)
                {
                    Console.WriteLine("No valid sell order found within the specified price.");
                    break;
                }

                Order* matchedSellOrder = &sellOrders[lowestSellIndex];
                int matchedQuantity = Math.Min(matchedSellOrder->Quantity, remainingQuantity);
                double cost = matchedQuantity * lowestSellPrice;

                if (buyer.Balance < cost)
                {
                    Console.WriteLine("Insufficient balance to buy at the lowest sell price.");
                    break;
                }

                matchedSellOrder->Quantity -= matchedQuantity;
                buyer.Balance -= cost;
                totalCost += cost;
                remainingQuantity -= matchedQuantity;

                Console.WriteLine($"Bought {matchedQuantity} units at {lowestSellPrice} each. Cost: {cost}. Remaining balance: {buyer.Balance}");

                if (matchedSellOrder->Quantity == 0)
                {
                    RemoveOrder(matchedSellOrder->Id);
                }
            }

            return buyQuantity != remainingQuantity; // true if any quantity was bought
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
                    orderIndex = i;
                    break;
                }
                else if (sellOrders[i].Id == orderId)
                {
                    targetOrders = sellOrders;
                    orderIndex = i;
                    break;
                }
            }
            if (targetOrders != null && orderIndex != -1)
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

        public unsafe void ModifyOrder(int orderId, double newPrice, int newQuantity)
        {
            for (int i = 0; i < size; i++)
            {
                if (buyOrders[i].Id == orderId)
                {
                    buyOrders[i].Price = newPrice;
                    buyOrders[i].Quantity = newQuantity;
                    UpdateAndNotify();
                    break;
                }
                else if (sellOrders[i].Id == orderId)
                {
                    sellOrders[i].Price = newPrice;
                    sellOrders[i].Quantity = newQuantity;
                    UpdateAndNotify();
                    break;
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
