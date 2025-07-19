namespace TradingPlatform
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<double> BuyTargetPrices { get; private set; }
        public List<double> SellTargetPrices { get; private set; }

        public User(int userId, string name)
        {
            UserId = userId;
            Name = name;
            BuyTargetPrices = new();
            SellTargetPrices = new();
        }

        public void SubscribeToBuyTargetPrice(double price)
        {
            BuyTargetPrices.Add(price);
        } 

        public void SubscribeToSellTargetPrice(double price)
        {
            SellTargetPrices.Add(price);
        }

        public void OnPriceNotification(object sender, PriceNotificationEventArgs e)
        {
            if (e.IsBuyOrder)
            {
                for(int i=BuyTargetPrices.Count - 1; i >= 0; i--)
                {
                    if (BuyTargetPrices[i] <=  e.Price)
                    {
                        Console.WriteLine($"User {Name} notified of buy target price: {e.Price}");
                        BuyTargetPrices.RemoveAt(i); // Remove after notification
                        break; // Exit after notifying for the first match
                    }
                }
            }
            else
            {
                for(int i=SellTargetPrices.Count - 1; i >= 0; i--)
                {
                    if (SellTargetPrices[i] >= e.Price && e.Price !=0)
                    {
                        Console.WriteLine($"User {Name} notified of sell target price: {e.Price}");
                        SellTargetPrices.RemoveAt(i); // Remove after notification
                        break; // Exit after notifying for the first match
                    }
                }
            }
        }
    }
}