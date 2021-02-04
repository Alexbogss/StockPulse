using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace StockPulse.Database.Entity
{
    public class StockEventEntity
    {
        [Key]
        public long Id { get; set; }

        public DateTime RaiseTime { get; set; }

        /* TODO event info ex: type */

        public StockEntity Stock { get; set; }

        public static void Setup(ModelBuilder builder)
        {
            var entity = builder.Entity<StockEventEntity>();

            entity.HasOne(_ => _.Stock)
                .WithMany(_ => _.Events)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
