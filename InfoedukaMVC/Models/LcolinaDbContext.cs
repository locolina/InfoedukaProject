using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InfoedukaMVC.Models;

public partial class LcolinaDbContext : DbContext
{
    public LcolinaDbContext()
    {
    }

    public LcolinaDbContext(DbContextOptions<LcolinaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<UserClassMapping> UserClassMappings { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    public virtual DbSet<VComment> VComments { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("InfoedukaDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__AppUser__1788CC4CEDAB8457");

            entity.ToTable("AppUser");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Pass).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

            entity.HasOne(d => d.UserType).WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AppUser__UserTyp__208CD6FA");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Class__CB1927A0270D3E75");

            entity.ToTable("Class");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassName).HasMaxLength(100);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__C3B4DFAA7B40D6CE");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.Content).HasMaxLength(2500);
            entity.Property(e => e.DatePosted).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Class).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__ClassID__2180FB33");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__UserID__22751F6C");
        });

        modelBuilder.Entity<UserClassMapping>(entity =>
        {
            entity.HasKey(e => e.UserClassMappingId).HasName("PK__UserClas__79A1E6E711CA25F9");

            entity.ToTable("UserClassMapping");

            entity.Property(e => e.UserClassMappingId).HasColumnName("UserClassMappingID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Class).WithMany(p => p.UserClassMappings)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserClass__Class__236943A5");

            entity.HasOne(d => d.User).WithMany(p => p.UserClassMappings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserClass__UserI__245D67DE");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__UserType__40D2D8F63A74863B");

            entity.ToTable("UserType");

            entity.Property(e => e.UserTypeId)
                .ValueGeneratedNever()
                .HasColumnName("UserTypeID");
            entity.Property(e => e.UserTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<VComment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vComment");

            entity.Property(e => e.ClassName).HasMaxLength(100);
            entity.Property(e => e.Content).HasMaxLength(2500);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
