using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockPulse.Database.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockPulse.Tests
{
    [TestClass]
    public class StockListHelperTest
    {
        [TestMethod]
        public async Task TestTickerRead()
        {
            var nasdaqPath = TestHelper.NasdaqStockListPath;
            var nasdaqTickerList = StockListHelper.ReadTickersFromFile(nasdaqPath);

            var nysePath = TestHelper.NyseStockListPath;
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
