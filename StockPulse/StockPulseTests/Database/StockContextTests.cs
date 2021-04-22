using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockPulse.Database;
using StockPulse.Database.Extensions;
using StockPulse.Tests.Reddit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                { "NyseStockList", RedditTestHelper.GetNyseStockListPath() },
                { "NasdaqStockList", RedditTestHelper.GetNasdaqStockListPath() }
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemConfig)
                .Build();

            await DatabasePrepareFactory.PrepareDb(db, config);
        }
    }
}
