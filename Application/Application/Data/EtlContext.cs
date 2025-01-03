using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public partial class EtlContext : DbContext
{
    public EtlContext()
    {
    }

    public EtlContext(DbContextOptions<EtlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-S7D9M3G\\SQLEXPRESS;Database=ETL;Integrated Security=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Trips__3213E83F70F22970");

            entity.HasIndex(e => new { e.PulocationId, e.TipAmount }, "PULocationID_TipAmount");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DolocationId).HasColumnName("DOLocationID");
            entity.Property(e => e.FareAmount)
                .HasColumnType("money")
                .HasColumnName("fare_amount");
            entity.Property(e => e.PassengerCount).HasColumnName("passenger_count");
            entity.Property(e => e.PulocationId).HasColumnName("PULocationID");
            entity.Property(e => e.StoreAndFwdFlag)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("store_and_fwd_flag");
            entity.Property(e => e.TipAmount)
                .HasColumnType("money")
                .HasColumnName("tip_amount");
            entity.Property(e => e.TpepDropoffDatetime).HasColumnName("tpep_dropoff_datetime");
            entity.Property(e => e.TpepPickupDatetime).HasColumnName("tpep_pickup_datetime");
            entity.Property(e => e.TripDistance).HasColumnName("trip_distance");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
