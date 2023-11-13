﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model;

#nullable disable

namespace Model.Migrations
{
    [DbContext(typeof(EfbierenContext))]
    [Migration("20231113144252_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Model.Bier", b =>
                {
                    b.Property<int>("BierNr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BierNr"));

                    b.Property<float?>("Alcohol")
                        .HasColumnType("real");

                    b.Property<int>("BrouwerNr")
                        .HasColumnType("int");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SoortNr")
                        .HasColumnType("int");

                    b.Property<byte[]>("SsmaTimeStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion")
                        .HasColumnName("SSMA_TimeStamp");

                    b.HasKey("BierNr")
                        .HasName("Bieren$PrimaryKey");

                    b.HasIndex("BrouwerNr");

                    b.HasIndex("SoortNr");

                    b.ToTable("Bieren");
                });

            modelBuilder.Entity("Model.Brouwer", b =>
                {
                    b.Property<int>("BrouwerNr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BrouwerNr"));

                    b.Property<string>("Adres")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("BrNaam")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gemeente")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Omzet")
                        .HasColumnType("int");

                    b.Property<short>("PostCode")
                        .HasColumnType("smallint");

                    b.HasKey("BrouwerNr")
                        .HasName("Brouwers$PrimaryKey");

                    b.ToTable("Brouwers");
                });

            modelBuilder.Entity("Model.Soort", b =>
                {
                    b.Property<int>("SoortNr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SoortNr"));

                    b.Property<string>("SoortNaam")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("SoortNr")
                        .HasName("Soorten$PrimaryKey");

                    b.ToTable("Soorten");
                });

            modelBuilder.Entity("Model.Bier", b =>
                {
                    b.HasOne("Model.Brouwer", "BrouwerNrNavigation")
                        .WithMany("Bieren")
                        .HasForeignKey("BrouwerNr")
                        .IsRequired()
                        .HasConstraintName("Bieren$BrouwersBieren");

                    b.HasOne("Model.Soort", "SoortNrNavigation")
                        .WithMany("Bieren")
                        .HasForeignKey("SoortNr")
                        .IsRequired()
                        .HasConstraintName("Bieren$SoortenBieren");

                    b.Navigation("BrouwerNrNavigation");

                    b.Navigation("SoortNrNavigation");
                });

            modelBuilder.Entity("Model.Brouwer", b =>
                {
                    b.Navigation("Bieren");
                });

            modelBuilder.Entity("Model.Soort", b =>
                {
                    b.Navigation("Bieren");
                });
#pragma warning restore 612, 618
        }
    }
}
