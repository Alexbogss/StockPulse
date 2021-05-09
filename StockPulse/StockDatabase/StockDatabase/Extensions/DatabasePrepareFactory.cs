using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace StockPulse.Database.Extensions
{
    public static class DatabasePrepareFactory
    {
        public static async Task PrepareDb(StockContext db, IConfiguration config)
        {
            await db.Database.MigrateAsync();

            if (!db.Stocks.Any())
            {
                foreach (var exchangeConfig in config.GetSection("ExchangeList").GetChildren())
                {
                    var tickers = StockListHelper.ReadTickersFromFile(exchangeConfig["Path"]);
                    await db.AddTickersToDbContext(tickers, exchangeConfig["Name"]);
                }
            }

            await db.SaveChangesAsync();
        }
    }
}
