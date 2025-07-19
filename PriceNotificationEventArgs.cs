namespace TradingPlatform
{
    public class PriceNotificationEventArgs
    {
        public double Price { get; }
        public bool IsBuyOrder { get; }

        public PriceNotificationEventArgs(double price, bool isBuyOrder)
        {
            if (price < 0)
            {
                throw new ArgumentException("Price cannot be negative", nameof(price));
            }

            Price = price;
            IsBuyOrder = isBuyOrder;
        }
    }
}