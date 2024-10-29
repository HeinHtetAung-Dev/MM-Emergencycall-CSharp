using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MMEmergencyCall.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EmergencyRequest> EmergencyRequests { get; set; }

    public virtual DbSet<EmergencyService> EmergencyServices { get; set; }

    public virtual DbSet<ServiceProvider> ServiceProviders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmergencyRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Emergenc__33A8517A65E25DA8");

            entity.HasIndex(e => e.ProviderId, "idx_ProviderId");

            entity.HasIndex(e => e.ServiceId, "idx_ServiceId");

            entity.HasIndex(e => e.UserId, "idx_UserId");

            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.RequestTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ResponseTime).HasColumnType("datetime");
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Township).HasMaxLength(100);

            entity.HasOne(d => d.Provider).WithMany(p => p.EmergencyRequests)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Emergency__Provi__2E1BDC42");

            entity.HasOne(d => d.Service).WithMany(p => p.EmergencyRequests)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Emergency__Servi__2D27B809");

            entity.HasOne(d => d.User).WithMany(p => p.EmergencyRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Emergency__UserI__2C3393D0");
        });

        modelBuilder.Entity<EmergencyService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Emergenc__C51BB00A372E00BB");

            entity.HasIndex(e => e.ServiceType, "idx_ServiceType");

            entity.Property(e => e.Availability)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.ServiceType).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(200);
            entity.Property(e => e.Township).HasMaxLength(200);
        });

        modelBuilder.Entity<ServiceProvider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PK__ServiceP__B54C687DE859F6DA");

            entity.Property(e => e.Availability)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ContactNumber).HasMaxLength(15);
            entity.Property(e => e.ProviderName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.Township).HasMaxLength(100);

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceProviders)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicePr__Servi__286302EC");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C78D5D195");

            entity.HasIndex(e => e.PhoneNumber, "idx_PhoneNumber");

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.EmergencyType).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Township)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
