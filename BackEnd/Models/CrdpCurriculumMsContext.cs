using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models;

public partial class CrdpCurriculumMsContext : DbContext
{
    public CrdpCurriculumMsContext()
    {
    }

    public CrdpCurriculumMsContext(DbContextOptions<CrdpCurriculumMsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Code> Codes { get; set; }

    public virtual DbSet<CodesContent> CodesContents { get; set; }

    public virtual DbSet<CompetenciesClass> CompetenciesClasses { get; set; }

    public virtual DbSet<CompetenciesConceptField> CompetenciesConceptFields { get; set; }

    public virtual DbSet<CompetenciesConceptTree> CompetenciesConceptTrees { get; set; }

    public virtual DbSet<CompetenciesCross> CompetenciesCrosses { get; set; }

    public virtual DbSet<CompetenciesDomain> CompetenciesDomains { get; set; }

    public virtual DbSet<CompetenciesStep> CompetenciesSteps { get; set; }

    public virtual DbSet<CompetenciesVersion> CompetenciesVersions { get; set; }

    public virtual DbSet<Competency> Competencies { get; set; }

    public virtual DbSet<ConceptTree> ConceptTrees { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleService> RoleServices { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserRolePermission> UserRolePermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=52.169.111.157; Database=CrdpCurriculumMS; User Id=sa; Password=CRDP@123; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AS");

        modelBuilder.Entity<Code>(entity =>
        {
            entity.Property(e => e.CodeDescription).HasMaxLength(255);
            entity.Property(e => e.CodeName).HasMaxLength(255);
        });

        modelBuilder.Entity<CodesContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Code_Content");

            entity.ToTable("CodesContent");

            entity.Property(e => e.CodeContentDescription).HasMaxLength(255);
            entity.Property(e => e.CodeContentName).HasMaxLength(255);

            entity.HasOne(d => d.Code).WithMany(p => p.CodesContents)
                .HasForeignKey(d => d.CodeId)
                .HasConstraintName("FK_Code_Content_Codes");
        });

        modelBuilder.Entity<CompetenciesClass>(entity =>
        {
            entity.ToTable("CompetenciesClass");

            entity.Property(e => e.Cid).HasColumnName("CId");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.CompetenciesClasses)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK_CompetenciesClass_Competencies");
        });

        modelBuilder.Entity<CompetenciesConceptField>(entity =>
        {
            entity.ToTable("CompetenciesConceptField");

            entity.Property(e => e.Cid).HasColumnName("CId");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.CompetenciesConceptFields)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK_CompetenciesConceptField_Competencies");
        });

        modelBuilder.Entity<CompetenciesConceptTree>(entity =>
        {
            entity.ToTable("CompetenciesConceptTree");

            entity.Property(e => e.Cid).HasColumnName("CId");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.CompetenciesConceptTrees)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK_CompetenciesConceptTree_Competencies");

            entity.HasOne(d => d.ConceptTree).WithMany(p => p.CompetenciesConceptTrees)
                .HasForeignKey(d => d.ConceptTreeId)
                .HasConstraintName("FK_CompetenciesConceptTree_ConceptTree");
        });

        modelBuilder.Entity<CompetenciesCross>(entity =>
        {
            entity.ToTable("CompetenciesCross");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CompMainId).HasColumnName("CompMainID");
            entity.Property(e => e.CompSubId).HasColumnName("CompSubID");

            entity.HasOne(d => d.CompMain).WithMany(p => p.CompetenciesCrossCompMains)
                .HasForeignKey(d => d.CompMainId)
                .HasConstraintName("FK_CompetenciesCross_CompetenciesMain");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.CompetenciesCrossIdNavigation)
                .HasForeignKey<CompetenciesCross>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompetenciesCross_CompetenciesSub");
        });

        modelBuilder.Entity<CompetenciesDomain>(entity =>
        {
            entity.ToTable("CompetenciesDomain");

            entity.Property(e => e.Cid).HasColumnName("CId");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.CompetenciesDomains)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK_CompetenciesDomain_Competencies");
        });

        modelBuilder.Entity<CompetenciesStep>(entity =>
        {
            entity.Property(e => e.Step).HasMaxLength(50);
            entity.Property(e => e.StepComment).HasMaxLength(500);
            entity.Property(e => e.StepDate).HasColumnType("datetime");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.CompetenciesSteps)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK_CompetenciesSteps_Competencies");
        });

        modelBuilder.Entity<CompetenciesVersion>(entity =>
        {
            entity.Property(e => e.CompetencyDetails).HasMaxLength(500);
            entity.Property(e => e.CompetencyName).HasMaxLength(500);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.IdNumber).HasMaxLength(50);
            entity.Property(e => e.VersionDateCreated).HasColumnType("datetime");
        });

        modelBuilder.Entity<Competency>(entity =>
        {
            entity.Property(e => e.CompetenceDetails).HasMaxLength(500);
            entity.Property(e => e.CompetenceName).HasMaxLength(500);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.IdNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<ConceptTree>(entity =>
        {
            entity.ToTable("ConceptTree");

            entity.Property(e => e.ConceptDetails).HasMaxLength(500);
            entity.Property(e => e.ConceptName).HasMaxLength(500);
            entity.Property(e => e.IdNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleDetails).HasMaxLength(500);
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<RoleService>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.RoleServices)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_RoleServices_Roles");

            entity.HasOne(d => d.Service).WithMany(p => p.RoleServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_RoleServices_Services");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(e => e.Clurl)
                .HasMaxLength(100)
                .HasColumnName("CLURL");
            entity.Property(e => e.Dependencies).HasMaxLength(500);
            entity.Property(e => e.HasChildren).HasColumnName("hasCHildren");
            entity.Property(e => e.Parent).HasMaxLength(100);
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.Svurl)
                .HasMaxLength(100)
                .HasColumnName("SVURL");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Details).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .HasColumnName("FName");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .HasColumnName("LName");
            entity.Property(e => e.Mname)
                .HasMaxLength(50)
                .HasColumnName("MName");
            entity.Property(e => e.Password).HasMaxLength(250);
            entity.Property(e => e.PhoneNb).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UserRoles_Roles");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserRoles_Users");
        });

        modelBuilder.Entity<UserRolePermission>(entity =>
        {
            entity.HasOne(d => d.UserRole).WithMany(p => p.UserRolePermissions)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("FK_UserRolePermissions_UserRoles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
