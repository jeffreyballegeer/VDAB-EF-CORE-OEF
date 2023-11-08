﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Entities;

#nullable disable

namespace Model.Migrations
{
    [DbContext(typeof(EFTakenContext))]
    partial class EFTakenContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Model.Entities.Artikel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArtikelType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ArtikelgroepId")
                        .HasColumnType("int");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArtikelgroepId");

                    b.ToTable("Artikels", (string)null);

                    b.HasDiscriminator<string>("ArtikelType").HasValue("Artikel");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Model.Entities.Artikelgroep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Artikelgroepen");
                });

            modelBuilder.Entity("Model.Entities.Klant", b =>
                {
                    b.Property<int>("KlantNr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KlantNr"));

                    b.Property<string>("Voornaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KlantNr");

                    b.ToTable("Klanten");

                    b.HasData(
                        new
                        {
                            KlantNr = 1,
                            Voornaam = "Marge"
                        },
                        new
                        {
                            KlantNr = 2,
                            Voornaam = "Homer"
                        },
                        new
                        {
                            KlantNr = 3,
                            Voornaam = "Lisa"
                        },
                        new
                        {
                            KlantNr = 4,
                            Voornaam = "Maggie"
                        },
                        new
                        {
                            KlantNr = 5,
                            Voornaam = "Simpson"
                        });
                });

            modelBuilder.Entity("Model.Entities.Personeelslid", b =>
                {
                    b.Property<int>("PersoneelsNr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersoneelsNr"));

                    b.Property<int?>("ManagerNr")
                        .HasColumnType("int");

                    b.Property<string>("Voornaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersoneelsNr");

                    b.HasIndex("ManagerNr");

                    b.ToTable("Personeelsleden");

                    b.HasData(
                        new
                        {
                            PersoneelsNr = 1,
                            Voornaam = "Diane"
                        },
                        new
                        {
                            PersoneelsNr = 2,
                            ManagerNr = 1,
                            Voornaam = "Mary"
                        },
                        new
                        {
                            PersoneelsNr = 3,
                            ManagerNr = 1,
                            Voornaam = "Jeff"
                        },
                        new
                        {
                            PersoneelsNr = 4,
                            ManagerNr = 2,
                            Voornaam = "William"
                        },
                        new
                        {
                            PersoneelsNr = 5,
                            ManagerNr = 2,
                            Voornaam = "Gerard"
                        },
                        new
                        {
                            PersoneelsNr = 6,
                            ManagerNr = 2,
                            Voornaam = "Anthony"
                        },
                        new
                        {
                            PersoneelsNr = 7,
                            ManagerNr = 6,
                            Voornaam = "Leslie"
                        },
                        new
                        {
                            PersoneelsNr = 8,
                            ManagerNr = 6,
                            Voornaam = "July"
                        },
                        new
                        {
                            PersoneelsNr = 9,
                            ManagerNr = 6,
                            Voornaam = "Steve"
                        },
                        new
                        {
                            PersoneelsNr = 10,
                            ManagerNr = 6,
                            Voornaam = "Foon Yue"
                        },
                        new
                        {
                            PersoneelsNr = 11,
                            ManagerNr = 6,
                            Voornaam = "George"
                        },
                        new
                        {
                            PersoneelsNr = 12,
                            ManagerNr = 5,
                            Voornaam = "Loui"
                        },
                        new
                        {
                            PersoneelsNr = 13,
                            ManagerNr = 5,
                            Voornaam = "Pamela"
                        },
                        new
                        {
                            PersoneelsNr = 14,
                            ManagerNr = 5,
                            Voornaam = "Larry"
                        },
                        new
                        {
                            PersoneelsNr = 15,
                            ManagerNr = 5,
                            Voornaam = "Barry"
                        },
                        new
                        {
                            PersoneelsNr = 16,
                            ManagerNr = 4,
                            Voornaam = "Andy"
                        },
                        new
                        {
                            PersoneelsNr = 17,
                            ManagerNr = 4,
                            Voornaam = "Peter"
                        },
                        new
                        {
                            PersoneelsNr = 18,
                            ManagerNr = 4,
                            Voornaam = "Tom"
                        },
                        new
                        {
                            PersoneelsNr = 19,
                            ManagerNr = 2,
                            Voornaam = "Mami"
                        },
                        new
                        {
                            PersoneelsNr = 20,
                            ManagerNr = 19,
                            Voornaam = "Yoshimi"
                        },
                        new
                        {
                            PersoneelsNr = 21,
                            ManagerNr = 5,
                            Voornaam = "Martin"
                        });
                });

            modelBuilder.Entity("Model.Entities.Rekening", b =>
                {
                    b.Property<string>("RekeningNr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("KlantNr")
                        .HasColumnType("int");

                    b.Property<decimal>("Saldo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Soort")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("RekeningNr");

                    b.HasIndex("KlantNr");

                    b.ToTable("Rekeningen");

                    b.HasData(
                        new
                        {
                            RekeningNr = "123-4567890-02",
                            KlantNr = 1,
                            Saldo = 1000m,
                            Soort = "Z"
                        },
                        new
                        {
                            RekeningNr = "234-5678901-69",
                            KlantNr = 1,
                            Saldo = 2000m,
                            Soort = "S"
                        },
                        new
                        {
                            RekeningNr = "345-6789012-12",
                            KlantNr = 2,
                            Saldo = 1000m,
                            Soort = "S"
                        });
                });

            modelBuilder.Entity("Model.Entities.FoodArtikel", b =>
                {
                    b.HasBaseType("Model.Entities.Artikel");

                    b.Property<int>("Houdbaarheid")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("F");
                });

            modelBuilder.Entity("Model.Entities.NonFoodArtikel", b =>
                {
                    b.HasBaseType("Model.Entities.Artikel");

                    b.Property<int>("Garantie")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("N");
                });

            modelBuilder.Entity("Model.Entities.Artikel", b =>
                {
                    b.HasOne("Model.Entities.Artikelgroep", "Artikelgroep")
                        .WithMany("Artikels")
                        .HasForeignKey("ArtikelgroepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artikelgroep");
                });

            modelBuilder.Entity("Model.Entities.Personeelslid", b =>
                {
                    b.HasOne("Model.Entities.Personeelslid", "Manager")
                        .WithMany("Ondergeschikten")
                        .HasForeignKey("ManagerNr");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Model.Entities.Rekening", b =>
                {
                    b.HasOne("Model.Entities.Klant", "Klant")
                        .WithMany("Rekeningen")
                        .HasForeignKey("KlantNr")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Klant");
                });

            modelBuilder.Entity("Model.Entities.Artikelgroep", b =>
                {
                    b.Navigation("Artikels");
                });

            modelBuilder.Entity("Model.Entities.Klant", b =>
                {
                    b.Navigation("Rekeningen");
                });

            modelBuilder.Entity("Model.Entities.Personeelslid", b =>
                {
                    b.Navigation("Ondergeschikten");
                });
#pragma warning restore 612, 618
        }
    }
}
