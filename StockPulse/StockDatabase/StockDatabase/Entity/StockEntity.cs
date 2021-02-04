using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockPulse.Database.Entity
{
    public class StockEntity
    {
        [Key]
        public long Id { get; set; }

        public string Ticker { get; set; }

        public string Exchange { get; set; }

        public DateTime LastUpdate { get; set; }

        public int TotalReferenceCount { get; set; }

        /* TODO index field/entity */

        public ICollection<StockEventEntity> Events { get; set; }

        public static void Setup(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<StockEntity>();

            entity.HasIndex(_ => _.Ticker).IsUnique();
        }
    }
}
