﻿// <auto-generated />
using System;
using AppiSimo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AppiSimo.Api.Migrations
{
    [DbContext(typeof(KingRogerContext))]
    partial class KingRogerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("AppiSimo.Shared.Model.Court", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Courts");
                });

            modelBuilder.Entity("AppiSimo.Shared.Model.CourtRate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CourtId");

                    b.Property<Guid>("RateId");

                    b.HasKey("Id");

                    b.HasIndex("CourtId");

                    b.HasIndex("RateId");

                    b.ToTable("CourtRate");
                });

            modelBuilder.Entity("AppiSimo.Shared.Model.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CourtId");

                    b.Property<DateTime>("EndDate");

                    b.Property<Guid?>("HeatId");

                    b.Property<Guid?>("LightId");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Users");

                    b.HasKey("Id");

                    b.HasIndex("CourtId");

                    b.HasIndex("HeatId");

                    b.HasIndex("LightId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("AppiSimo.Shared.Model.Heat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Default");

                    b.Property<string>("HeatType");

                    b.Property<decimal>("Price");

                    b.Property<int>("Priority");

                    b.HasKey("Id");

                    b.ToTable("Heats");
                });

            modelBuilder.Entity("AppiSimo.Shared.Model.Light", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Default");

                    b.Property<string>("LightType");

                    b.Property<decimal>("Price");

                    b.Property<int>("Priority");

                    b.HasKey("Id");

                    b.ToTable("Lights");
                });

            modelBuilder.Entity("AppiSimo.Shared.Model.Rate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndHour");

                    b.Property<decimal>("Price");

                    b.Property<DateTime>("StartDate");

                    b.Property<DateTime>("StartHour");

                    b.HasKey("Id");

                    b.ToTable("Rates");
                });

            modelBuilder.Entity("AppiSimo.Shared.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AppiSimo.Shared.Model.UserEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("EventId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("UserEvent");
                });

            modelBuilder.Entity("AppiSimo.Shared.Model.CourtRate", b =>
                {
                    b.HasOne("AppiSimo.Shared.Model.Court", "Court")
                        .WithMany("CourtsRates")
                        .HasForeignKey("CourtId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AppiSimo.Shared.Model.Rate", "Rate")
                        .WithMany("CourtsRates")
                        .HasForeignKey("RateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AppiSimo.Shared.Model.Event", b =>
                {
                    b.HasOne("AppiSimo.Shared.Model.Court", "Court")
                        .WithMany()
                        .HasForeignKey("CourtId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AppiSimo.Shared.Model.Heat", "Heat")
                        .WithMany()
                        .HasForeignKey("HeatId");

                    b.HasOne("AppiSimo.Shared.Model.Light", "Light")
                        .WithMany()
                        .HasForeignKey("LightId");
                });

            modelBuilder.Entity("AppiSimo.Shared.Model.UserEvent", b =>
                {
                    b.HasOne("AppiSimo.Shared.Model.Event", "Event")
                        .WithMany("UsersEvents")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AppiSimo.Shared.Model.User", "User")
                        .WithMany("UsersEvents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
