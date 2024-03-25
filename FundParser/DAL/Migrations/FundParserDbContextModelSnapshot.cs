﻿// <auto-generated />
using System;
using FundParser.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FundParser.DAL.Migrations
{
    [DbContext(typeof(FundParserDbContext))]
    partial class FundParserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("FundParser.DAL.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cusip")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Companies", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Cusip = "CUSIP1",
                            Name = "Company1",
                            Ticker = "TICK1"
                        });
                });

            modelBuilder.Entity("FundParser.DAL.Models.Fund", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Funds", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Fund1"
                        });
                });

            modelBuilder.Entity("FundParser.DAL.Models.Holding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("FundId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("MarketValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Shares")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Weight")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("FundId");

                    b.ToTable("Holdings", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CompanyId = 1,
                            Date = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FundId = 1,
                            MarketValue = 50000m,
                            Shares = 1000m,
                            Weight = 0.05m
                        },
                        new
                        {
                            Id = 2,
                            CompanyId = 1,
                            Date = new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FundId = 1,
                            MarketValue = 100000m,
                            Shares = 2000m,
                            Weight = 0.1m
                        });
                });

            modelBuilder.Entity("FundParser.DAL.Models.HoldingDiff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("FundId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("NewHoldingDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NewHoldingId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OldHoldingDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("OldHoldingId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("OldShares")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OldWeight")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SharesChange")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("WeightChange")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("FundId");

                    b.HasIndex("NewHoldingId");

                    b.HasIndex("OldHoldingId");

                    b.ToTable("HoldingDiffs", (string)null);
                });

            modelBuilder.Entity("FundParser.DAL.Models.Holding", b =>
                {
                    b.HasOne("FundParser.DAL.Models.Company", "Company")
                        .WithMany("Holdings")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FundParser.DAL.Models.Fund", "Fund")
                        .WithMany("Holdings")
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Fund");
                });

            modelBuilder.Entity("FundParser.DAL.Models.HoldingDiff", b =>
                {
                    b.HasOne("FundParser.DAL.Models.Company", "Company")
                        .WithMany("HoldingDiffs")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FundParser.DAL.Models.Fund", "Fund")
                        .WithMany("HoldingDiffs")
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FundParser.DAL.Models.Holding", "NewHolding")
                        .WithMany()
                        .HasForeignKey("NewHoldingId");

                    b.HasOne("FundParser.DAL.Models.Holding", "OldHolding")
                        .WithMany()
                        .HasForeignKey("OldHoldingId");

                    b.Navigation("Company");

                    b.Navigation("Fund");

                    b.Navigation("NewHolding");

                    b.Navigation("OldHolding");
                });

            modelBuilder.Entity("FundParser.DAL.Models.Company", b =>
                {
                    b.Navigation("HoldingDiffs");

                    b.Navigation("Holdings");
                });

            modelBuilder.Entity("FundParser.DAL.Models.Fund", b =>
                {
                    b.Navigation("HoldingDiffs");

                    b.Navigation("Holdings");
                });
#pragma warning restore 612, 618
        }
    }
}
