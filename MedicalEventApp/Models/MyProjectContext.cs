using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedicalEventApp.Models;

public partial class MyProjectContext : DbContext
{
    public MyProjectContext()
    {
    }

    public MyProjectContext(DbContextOptions<MyProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<DeptTable> DeptTables { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Er> Ers { get; set; }

    public virtual DbSet<Inf> Infs { get; set; }

    public virtual DbSet<Inpt> Inpts { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Opd> Opds { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectMember> ProjectMembers { get; set; }

    public virtual DbSet<ProjectMethod> ProjectMethods { get; set; }

    public virtual DbSet<ProjectNotify> ProjectNotifies { get; set; }

    public virtual DbSet<TimeTable> TimeTables { get; set; }

    public virtual DbSet<ViewOpdDoctorMonthly> ViewOpdDoctorMonthlies { get; set; }

    public virtual DbSet<Ym> Yms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.Property(e => e.Cartid).HasColumnName("cartid");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Memberid).HasColumnName("memberid");
            entity.Property(e => e.Pid).HasColumnName("pid");

            entity.HasOne(d => d.Member).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Memberid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Employees");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Product");
        });

        modelBuilder.Entity<DeptTable>(entity =>
        {
            entity.HasKey(e => e.DeptId);

            entity.ToTable("DeptTable");

            entity.Property(e => e.DeptId)
                .ValueGeneratedNever()
                .HasColumnName("DeptID");
            entity.Property(e => e.DeptName).HasMaxLength(50);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Empid);

            entity.Property(e => e.Empid)
                .ValueGeneratedNever()
                .HasColumnName("EMPId");
            entity.Property(e => e.Dept).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Profession).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<Er>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ER");

            entity.Property(e => e.AdmittedVisits).HasColumnName("Admitted_Visits");
            entity.Property(e => e.BoardingVisits).HasColumnName("Boarding_Visits");
            entity.Property(e => e.ErTotalVisits).HasColumnName("ER_Total_Visits");
            entity.Property(e => e.TriageVisits).HasColumnName("Triage_Visits");
            entity.Property(e => e.YearMonth).HasMaxLength(6);

            entity.HasOne(d => d.YearMonthNavigation).WithMany()
                .HasForeignKey(d => d.YearMonth)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ER_YM");
        });

        modelBuilder.Entity<Inf>(entity =>
        {
            entity.HasKey(e => e.YearMonth);

            entity.ToTable("INF");

            entity.Property(e => e.YearMonth).HasMaxLength(6);
            entity.Property(e => e.AttendingPhysician)
                .HasMaxLength(20)
                .HasColumnName("Attending_Physician");
            entity.Property(e => e.BedDaysContext).HasColumnName("BedDays_Context");
            entity.Property(e => e.Department).HasMaxLength(20);
            entity.Property(e => e.InfectionSite)
                .HasMaxLength(50)
                .HasColumnName("Infection_Site");
            entity.Property(e => e.Pathogen).HasMaxLength(50);
            entity.Property(e => e.PatientId)
                .HasMaxLength(15)
                .HasColumnName("PatientID");

            entity.HasOne(d => d.YearMonthNavigation).WithOne(p => p.Inf)
                .HasForeignKey<Inf>(d => d.YearMonth)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_INF_YM");
        });

        modelBuilder.Entity<Inpt>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("INPT");

            entity.Property(e => e.Alos)
                .HasColumnType("decimal(3, 1)")
                .HasColumnName("ALOS");
            entity.Property(e => e.Department).HasMaxLength(20);
            entity.Property(e => e.OccRate).HasColumnType("decimal(5, 4)");
            entity.Property(e => e.YearMonth).HasMaxLength(6);

            entity.HasOne(d => d.YearMonthNavigation).WithMany()
                .HasForeignKey(d => d.YearMonth)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_INPT_YM");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Topic).HasMaxLength(50);
        });

        modelBuilder.Entity<Opd>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OPD");

            entity.Property(e => e.Account)
                .HasMaxLength(50)
                .HasColumnName("ACCOUNT");
            entity.Property(e => e.DeptId).HasColumnName("DeptID");
            entity.Property(e => e.IsFirstTime).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OpdDate).HasMaxLength(50);
            entity.Property(e => e.Opddept)
                .HasMaxLength(50)
                .HasColumnName("OPDDept");
            entity.Property(e => e.OpddocName)
                .HasMaxLength(50)
                .HasColumnName("OPDDocName");
            entity.Property(e => e.OpdsubDept)
                .HasMaxLength(50)
                .HasColumnName("OPDSubDept");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Pid);

            entity.ToTable("Product");

            entity.Property(e => e.Pid)
                .ValueGeneratedNever()
                .HasColumnName("pid");
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .HasColumnName("author");
            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.Pname)
                .HasMaxLength(50)
                .HasColumnName("pname");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Publisher)
                .HasMaxLength(50)
                .HasColumnName("publisher");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project");

            entity.Property(e => e.FileName).HasMaxLength(50);
            entity.Property(e => e.Indicators).HasMaxLength(50);
            entity.Property(e => e.Photo).HasColumnType("image");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<ProjectMember>(entity =>
        {
            entity.ToTable("ProjectMember");

            entity.Property(e => e.TeamMid).HasColumnName("TeamMId");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectMembers)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectMember_Project");

            entity.HasOne(d => d.TeamM).WithMany(p => p.ProjectMembers)
                .HasForeignKey(d => d.TeamMid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectMember_Employees");
        });

        modelBuilder.Entity<ProjectMethod>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ProjectMethod");

            entity.Property(e => e.Method).HasMaxLength(50);
            entity.Property(e => e.Step).HasMaxLength(50);

            entity.HasOne(d => d.Project).WithMany()
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectMethod_Project");
        });

        modelBuilder.Entity<ProjectNotify>(entity =>
        {
            entity.ToTable("ProjectNotify");

            entity.Property(e => e.Message).HasMaxLength(50);

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectNotifies)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_ProjectNotify_Project");
        });

        modelBuilder.Entity<TimeTable>(entity =>
        {
            entity.HasKey(e => e.DataDate);

            entity.ToTable("TimeTable");

            entity.Property(e => e.DataDate).HasMaxLength(50);
            entity.Property(e => e.IsWorkingDay).HasMaxLength(50);
            entity.Property(e => e.YearMonth).HasMaxLength(6);
            entity.Property(e => e.YearQuater).HasMaxLength(50);
        });

        modelBuilder.Entity<ViewOpdDoctorMonthly>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_OPD_Doctor_Monthly");

            entity.Property(e => e.DeptId).HasColumnName("DeptID");
            entity.Property(e => e.FirstVisitRate).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Opddept)
                .HasMaxLength(50)
                .HasColumnName("OPDDept");
            entity.Property(e => e.OpddocName)
                .HasMaxLength(50)
                .HasColumnName("OPDDocName");
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.YearMonth).HasMaxLength(6);
            entity.Property(e => e.YearQuater).HasMaxLength(50);
        });

        modelBuilder.Entity<Ym>(entity =>
        {
            entity.HasKey(e => e.Yearmonth);

            entity.ToTable("YM");

            entity.Property(e => e.Yearmonth).HasMaxLength(6);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
