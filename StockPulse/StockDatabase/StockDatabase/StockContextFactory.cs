using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq;

namespace StockPulse.Database
{
    public class StockContextFactory : IDesignTimeDbContextFactory<StockContext>
    {
        public StockContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockContext>();

            var connectionString = args?.Any() == true 
                ? args.First() 
                : "Data Source=D:\\CustomsTestDb\\testDb.db;";

            optionsBuilder.UseSqlite(connectionString);

            return new StockContext(optionsBuilder.Options);
        }
    }
}
