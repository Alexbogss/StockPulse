using StockPulse.Database.Entity;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StockPulse.Database.Extensions
{
    public static class StockListHelper
    {
        public static async IAsyncEnumerable<string> ReadTickersFromFile(string filePath)
        {
            using var reader = new StreamReader(filePath);

            string line;
            while((line = await reader.ReadLineAsync()) != null) {
                var ticker = line.Split('\t')[0];

                yield return ticker;
            }
        }

        public static async Task AddTickersToDbContext(this StockContext context, IAsyncEnumerable<string> tickers, string exchange)
        {
            await foreach(var ticker in tickers)
            {
                await context.Stocks.AddAsync(new StockEntity
                {
                    Ticker = ticker,
                    Exchange = exchange
                });
            }
        }
    }
}
