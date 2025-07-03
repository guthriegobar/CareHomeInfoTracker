using System;
using System.Collections.Generic;
using CareHomeInfoTracker.Models.TempModels;
using Microsoft.EntityFrameworkCore;

namespace CareHomeInfoTracker.Data.TempData;

public partial class CareHomeInfoContext : DbContext
{
    public CareHomeInfoContext()
    {
    }

    public CareHomeInfoContext(DbContextOptions<CareHomeInfoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Resident> Residents { get; set; }

    public virtual DbSet<SystemUser> SystemUsers { get; set; }

    public virtual DbSet<WeightHistory> WeightHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=CareHomeInfo;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Resident>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Resident_pkey");

            entity.ToTable("Resident");

            entity.HasIndex(e => new { e.RoomNum, e.BedNum }, "room_bed").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BedNum)
                .HasColumnType("character varying")
                .HasColumnName("bed_num");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.ImgLocation)
                .HasColumnType("character varying")
                .HasColumnName("img_location");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasColumnType("character varying")
                .HasColumnName("middle_name");
            entity.Property(e => e.RoomNum)
                .HasColumnType("character varying")
                .HasColumnName("room_num");
        });

        modelBuilder.Entity<SystemUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("System_User_pkey");

            entity.ToTable("System_User");

            entity.Property(e => e.Id).HasColumnType("character varying");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.Password).HasColumnType("character varying");
            entity.Property(e => e.Role).HasColumnType("character varying");
        });

        modelBuilder.Entity<WeightHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Weight_History_pkey");

            entity.ToTable("Weight_History");

            entity.HasIndex(e => new { e.ResidentId, e.RecordedDate }, "unique_day").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RecordedDate).HasColumnName("recorded_date");
            entity.Property(e => e.ResidentId).HasColumnName("resident_id");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.Resident).WithMany(p => p.WeightHistories)
                .HasForeignKey(d => d.ResidentId)
                .HasConstraintName("Weight_History_resident_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
