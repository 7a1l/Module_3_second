using System;
using System.Collections.Generic;
using MOdule3.Models;
using Microsoft.EntityFrameworkCore;

namespace MOdule3.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("database=module3;port=5432;username=postgres;password=123456;host=localhost;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isadmin)
                .HasDefaultValue(false)
                .HasColumnName("isadmin");
            entity.Property(e => e.Isblocked)
                .HasDefaultValue(false)
                .HasColumnName("isblocked");
            entity.Property(e => e.Isfirstlogin)
                .HasDefaultValue(true)
                .HasColumnName("isfirstlogin");
            entity.Property(e => e.Lastlogin)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastlogin");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Totalattempts)
                .HasDefaultValue(0)
                .HasColumnName("totalattempts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
