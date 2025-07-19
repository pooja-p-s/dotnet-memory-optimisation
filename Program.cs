using System;

namespace TradingPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            TradeLogger tradeLogger = new TradeLogger();

            Random rand = new Random();

            for (int i = 0; i < 10; i++)
            {
                Trade trade = new Trade
                {
                    TradeId = i + 1,
                    OrderId = (i%50) + 1,
                    Price = 100 + (i*20),
                    Quantity = 10 + (i%5),
                    Timestamp = DateTime.Now.AddDays(-rand.Next(1, 30))
                };

                tradeLogger.LogTrade(trade);
            } 

            tradeLogger.FinalizeLogging();
        }
    }
}