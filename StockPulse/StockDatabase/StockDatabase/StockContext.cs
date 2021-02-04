using Microsoft.EntityFrameworkCore;
using StockPulse.Database.Entity;

namespace StockPulse.Database
{
    public class StockContext : DbContext
    {
        public DbSet<StockEntity> Stocks { get; set; }

        public DbSet<StockEventEntity> StockEvents { get; set; }

        public StockContext(DbContextOptions<StockContext> options) : base(options) { }

        public StockContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            StockEntity.Setup(modelBuilder);
            StockEventEntity.Setup(modelBuilder);
        }
    }
}
