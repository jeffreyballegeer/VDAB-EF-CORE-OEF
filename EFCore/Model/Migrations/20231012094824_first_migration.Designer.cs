﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Entitites;

#nullable disable

namespace Model.Migrations
{
    [DbContext(typeof(EFOpleidingenContext))]
    [Migration("20231012094824_first_migration")]
    partial class first_migration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Model.Entitites.Campus", b =>
                {
                    b.Property<int>("CampusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CampusId"));

                    b.Property<string>("Gemeente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Huisnummer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Straat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CampusId");

                    b.ToTable("Campussen");
                });

            modelBuilder.Entity("Model.Entitites.Docent", b =>
                {
                    b.Property<int>("DocentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocentId"));

                    b.Property<int>("CampusId")
                        .HasColumnType("int");

                    b.Property<string>("Familienaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Voornaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Wedde")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("DocentId");

                    b.HasIndex("CampusId");

                    b.ToTable("Docenten");
                });

            modelBuilder.Entity("Model.Entitites.Docent", b =>
                {
                    b.HasOne("Model.Entitites.Campus", "Campus")
                        .WithMany("Docenten")
                        .HasForeignKey("CampusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Campus");
                });

            modelBuilder.Entity("Model.Entitites.Campus", b =>
                {
                    b.Navigation("Docenten");
                });
#pragma warning restore 612, 618
        }
    }
}
