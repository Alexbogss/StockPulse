using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace StockPulse.Database.Extensions
{
    public static class DatabasePrepareFactory
    {
        public static async Task PrepareDb(StockContext context, IConfiguration config)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();

            var nyseTickers = StockListHelper.ReadTickersFromFile(config["NyseStockList"]);
            await context.AddTickersToDbContext(nyseTickers, "NYSE");

            var nasdaqTickers = StockListHelper.ReadTickersFromFile(config["NasdaqStockList"]);
            await context.AddTickersToDbContext(nasdaqTickers, "NASDAQ");

            await context.SaveChangesAsync();
        }
    }
}
