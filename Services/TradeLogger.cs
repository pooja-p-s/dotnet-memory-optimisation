using System.Runtime.InteropServices;

namespace TradingPlatform
{
    public unsafe class TradeLogger
    {
        const int bufferSize = 100; // Size of the buffer for reading trades
        Trade* tradeBuffer;

        int tradeCount;

        public TradeLogger()
        {
            unsafe{
                tradeBuffer = (Trade*)NativeMemory.Alloc((nuint)(bufferSize * sizeof(Trade)));
            }
        }

        ~TradeLogger()
        {
            unsafe{
                NativeMemory.Free(tradeBuffer);
            }
        }

        public unsafe void LogTrade(Trade trade)
        {
            if (tradeCount < bufferSize)
            {
                tradeBuffer[tradeCount] = trade;
                tradeCount++;
            }
            else
            {
                ProcesssAndClearBuffer();
                tradeBuffer[0] = trade; // Add the new trade to the start of the
                tradeCount = 1; // Reset count to 1
            }
        }

        public void ProcesssAndClearBuffer()
        {
            using(StreamWriter writer = new StreamWriter("trades.log", true))
            {
                for (int i = 0; i < tradeCount; i++)
                {
                    Trade trade = tradeBuffer[i];
                    writer.WriteLine($"TradeId: {trade.TradeId}, OrderId: {trade.OrderId}, Price: {trade.Price}, Quantity: {trade.Quantity}, Timestamp: {trade.Timestamp}");
                }
            }
            tradeCount = 0; // Reset the count after processing
        }

        public unsafe void FinalizeLogging()
        {
            if (tradeCount > 0)
            {
                ProcesssAndClearBuffer();
            }
        }   
    }
}