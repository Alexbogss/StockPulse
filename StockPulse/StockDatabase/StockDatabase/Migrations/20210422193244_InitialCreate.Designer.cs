﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockPulse.Database;

namespace StockPulse.Database.Migrations
{
    [DbContext(typeof(StockContext))]
    [Migration("20210422193244_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("StockPulse.Database.Entity.StockEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Exchange")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ticker")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalReferenceCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Ticker")
                        .IsUnique();

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("StockPulse.Database.Entity.StockEventEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("RaiseTime")
                        .HasColumnType("TEXT");

                    b.Property<long>("StockId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.ToTable("StockEvents");
                });

            modelBuilder.Entity("StockPulse.Database.Entity.StockEventEntity", b =>
                {
                    b.HasOne("StockPulse.Database.Entity.StockEntity", "Stock")
                        .WithMany("Events")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockPulse.Database.Entity.StockEntity", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}