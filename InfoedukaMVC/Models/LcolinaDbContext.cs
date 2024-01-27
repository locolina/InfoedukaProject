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

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<UserCourseMapping> UserCourseMappings { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:InfoedukaDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__AppUser__1788CC4C64681528");

            entity.ToTable("AppUser");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Pass).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

            entity.HasOne(d => d.UserType).WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AppUser__UserTyp__589C25F3");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__C3B4DFAA86A62A0B");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.Content).HasMaxLength(2500);
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.DatePosted).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Course).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__CourseI__59904A2C");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__UserID__5A846E65");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71878476D733");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserCourseMapping>(entity =>
        {
            entity.HasKey(e => e.UserCourseMappingId).HasName("PK__UserCour__0C22E81FF701CEDB");

            entity.ToTable("UserCourseMapping");

            entity.Property(e => e.UserCourseMappingId).HasColumnName("UserCourseMappingID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Course).WithMany(p => p.UserCourseMappings)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCours__Cours__5B78929E");

            entity.HasOne(d => d.User).WithMany(p => p.UserCourseMappings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCours__UserI__5C6CB6D7");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__UserType__40D2D8F654311AE2");

            entity.ToTable("UserType");

            entity.Property(e => e.UserTypeId)
                .ValueGeneratedNever()
                .HasColumnName("UserTypeID");
            entity.Property(e => e.UserTypeName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
