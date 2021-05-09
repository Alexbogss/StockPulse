using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockPulse.Database;
using StockPulse.Database.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockPulse.Tests.Database
{
    [TestClass]
    public class StockContextTests
    {
        [TestMethod]
        public async Task PrepareDbTest()
        {
            var db = new StockContextFactory().CreateDbContext(null);

            var inMemConfig = new Dictionary<string, string>
            {
                { "ExchangeList:0:Name", "NYSE" },
                { "ExchangeList:0:Path", TestHelper.NyseStockListPath },
                { "ExchangeList:1:Name", "NASDAQ" },
                { "ExchangeList:1:Path", TestHelper.NasdaqStockListPath }
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemConfig)
                .Build();

            await DatabasePrepareFactory.PrepareDb(db, config);

            Assert.IsTrue(db.Stocks.Any());

            await db.Database.EnsureDeletedAsync();
        }
    }
}
