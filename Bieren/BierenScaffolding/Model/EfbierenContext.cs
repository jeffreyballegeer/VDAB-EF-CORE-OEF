using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Model;

public partial class EfbierenContext : DbContext
{
    public EfbierenContext()
    {
    }

    public EfbierenContext(DbContextOptions<EfbierenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bier> Bieren { get; set; }

    public virtual DbSet<Brouwer> Brouwers { get; set; }

    public virtual DbSet<Soort> Soorten { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=EFBieren;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bier>(entity =>
        {
            entity.HasKey(e => e.BierNr).HasName("Bieren$PrimaryKey");

            entity.Property(e => e.Naam).HasMaxLength(100);
            entity.Property(e => e.SsmaTimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("SSMA_TimeStamp");

            entity.HasOne(d => d.BrouwerNrNavigation).WithMany(p => p.Bieren)
                .HasForeignKey(d => d.BrouwerNr)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Bieren$BrouwersBieren");

            entity.HasOne(d => d.SoortNrNavigation).WithMany(p => p.Bieren)
                .HasForeignKey(d => d.SoortNr)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Bieren$SoortenBieren");
        });

        modelBuilder.Entity<Brouwer>(entity =>
        {
            entity.HasKey(e => e.BrouwerNr).HasName("Brouwers$PrimaryKey");

            entity.Property(e => e.Adres).HasMaxLength(50);
            entity.Property(e => e.BrNaam).HasMaxLength(50);
            entity.Property(e => e.Gemeente).HasMaxLength(50);
        });

        modelBuilder.Entity<Soort>(entity =>
        {
            entity.HasKey(e => e.SoortNr).HasName("Soorten$PrimaryKey");

            entity.Property(e => e.SoortNaam).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
