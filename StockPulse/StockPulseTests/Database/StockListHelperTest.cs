using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockPulse.Database.Extensions;
using StockPulse.Tests.Reddit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPulse.Tests
{
    [TestClass]
    public class StockListHelperTest
    {
        [TestMethod]
        public async Task TestTickerRead()
        {
            var nasdaqPath = RedditTestHelper.GetNasdaqStockListPath();
            var nasdaqTickerList = StockListHelper.ReadTickersFromFile(nasdaqPath);

            var nysePath = RedditTestHelper.GetNyseStockListPath();
            var nyseTickerList = StockListHelper.ReadTickersFromFile(nysePath);

            var result = new List<string>();

            await foreach (var ticker in nasdaqTickerList)
            {
                result.Add(ticker);
            }

            await foreach (var ticker in nyseTickerList)
            {
                result.Add(ticker);
            }

            var duplicate = result
                .GroupBy(_ => _)
                .Where(g => g.Count() > 1)
                .Select(_ => _)
                .ToArray();

            Assert.IsFalse(duplicate.Any());

            Assert.IsTrue(result.Any());
        }
    }
}
