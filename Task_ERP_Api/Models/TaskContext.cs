﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Task_ERP_Api.Models;

public partial class TaskContext : DbContext
{
    public TaskContext()
    {
    }

    public TaskContext(DbContextOptions<TaskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<JV> JVs { get; set; }

    public virtual DbSet<JVDetail> JVDetails { get; set; }

    public virtual DbSet<JVType> JVTypes { get; set; }

    public virtual DbSet<SubAccount> SubAccounts { get; set; }

    public virtual DbSet<SubAccountsClient> SubAccountsClients { get; set; }

    public virtual DbSet<SubAccountsType> SubAccountsTypes { get; set; }

    public virtual DbSet<SubAccounts_Detail> SubAccounts_Details { get; set; }

    public virtual DbSet<SubAccounts_Level> SubAccounts_Levels { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountID).HasAnnotation("SqlServer:FillFactor", 80);

            entity.HasIndex(e => e.AccountNumber, "IX_Accounts")
                .IsUnique()
                .HasFillFactor(80);

            entity.Property(e => e.AccountID).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Branch).WithMany(p => p.Accounts).HasConstraintName("FK_Accounts_Branches");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchID).HasAnnotation("SqlServer:FillFactor", 80);

            entity.Property(e => e.BranchID).ValueGeneratedNever();
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityID).HasAnnotation("SqlServer:FillFactor", 80);

            entity.Property(e => e.CityID).ValueGeneratedNever();

            entity.HasOne(d => d.Branch).WithMany(p => p.Cities).HasConstraintName("FK_Cities_Branches");
        });

        modelBuilder.Entity<JV>(entity =>
        {
            entity.HasKey(e => e.JVID)
                .HasName("PK_GL")
                .HasAnnotation("SqlServer:FillFactor", 80);

            entity.Property(e => e.JVID).ValueGeneratedNever();

            entity.HasOne(d => d.Branch).WithMany(p => p.JVs).HasConstraintName("FK_JV_Branches");

            entity.HasOne(d => d.JVType).WithMany(p => p.JVs).HasConstraintName("FK_JV_JVTypes");
        });

        modelBuilder.Entity<JVDetail>(entity =>
        {
            entity.HasKey(e => e.JVDetailID).HasAnnotation("SqlServer:FillFactor", 80);

            entity.Property(e => e.JVDetailID).ValueGeneratedOnAdd().UseIdentityColumn();

            entity.HasOne(d => d.Account).WithMany(p => p.JVDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JVDetails_Accounts");

            entity.HasOne(d => d.Branch).WithMany(p => p.JVDetails).HasConstraintName("FK_JVDetails_Branches");

            entity.HasOne(d => d.JV).WithMany(p => p.JVDetails).HasConstraintName("FK_JVDetails_A_JV1");

            entity.HasOne(d => d.SubAccount).WithMany(p => p.JVDetails).HasConstraintName("FK_JVDetails_SubAccounts");
        });

        modelBuilder.Entity<JVType>(entity =>
        {
            entity.HasKey(e => e.JVTypeID).HasAnnotation("SqlServer:FillFactor", 80);

            entity.Property(e => e.JVTypeID).ValueGeneratedNever();

            entity.HasOne(d => d.Branch).WithMany(p => p.JVTypes).HasConstraintName("FK_JVTypes_Branches");
        });

        modelBuilder.Entity<SubAccount>(entity =>
        {
            entity.HasKey(e => e.SubAccountID).HasAnnotation("SqlServer:FillFactor", 80);

            entity.HasIndex(e => e.SubAccountNumber, "IX_SubAccounts")
                .IsUnique()
                .HasFillFactor(80);

            entity.Property(e => e.SubAccountID).ValueGeneratedNever();

            entity.HasOne(d => d.Branch).WithMany(p => p.SubAccounts).HasConstraintName("FK_SubAccounts_Branches");

            entity.HasOne(d => d.Level).WithMany(p => p.SubAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubAccounts_SubAccounts_Levels");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_SubAccounts_SubAccounts");

            entity.HasOne(d => d.SubAccountType).WithMany(p => p.SubAccounts).HasConstraintName("FK_SubAccounts_SubAccountsTypes");
        });

        modelBuilder.Entity<SubAccountsClient>(entity =>
        {
            entity.HasKey(e => e.ClientID)
                .HasName("PK_Clients")
                .HasAnnotation("SqlServer:FillFactor", 80);

            entity.Property(e => e.ClientID).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.SubAccountsClients).HasConstraintName("FK_SubAccountsClient_Branches");

            entity.HasOne(d => d.City).WithMany(p => p.SubAccountsClients).HasConstraintName("FK_SubAccountsClient_Cities");

            entity.HasOne(d => d.SubAccount).WithMany(p => p.SubAccountsClients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubAccountsClient_SubAccounts");
        });

        modelBuilder.Entity<SubAccountsType>(entity =>
        {
            entity.HasKey(e => e.SubAccountTypeID).HasAnnotation("SqlServer:FillFactor", 80);

            entity.Property(e => e.SubAccountTypeID).ValueGeneratedNever();
        });

        modelBuilder.Entity<SubAccounts_Detail>(entity =>
        {
            entity.HasKey(e => e.SubAccountDetailID)
                .HasName("PK_A_SubAccounts_Details")
                .HasAnnotation("SqlServer:FillFactor", 80);

            entity.Property(e => e.SubAccountDetailID).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.SubAccounts_Details)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubAccounts_Details_Accounts");

            entity.HasOne(d => d.Branch).WithMany(p => p.SubAccounts_Details).HasConstraintName("FK_SubAccounts_Details_Branches");

            entity.HasOne(d => d.SubAccount).WithMany(p => p.SubAccounts_Details)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubAccounts_Details_SubAccounts");
        });

        modelBuilder.Entity<SubAccounts_Level>(entity =>
        {
            entity.HasKey(e => e.LevelID)
                .HasName("PK_SubAccLevels")
                .HasAnnotation("SqlServer:FillFactor", 80);

            entity.Property(e => e.LevelID).ValueGeneratedNever();

            entity.HasOne(d => d.Branch).WithMany(p => p.SubAccounts_Levels).HasConstraintName("FK_SubAccounts_Levels_Branches");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}