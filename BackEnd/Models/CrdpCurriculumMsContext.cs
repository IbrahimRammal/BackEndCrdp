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

    public virtual DbSet<Competencies> Competencies { get; set; }

    public virtual DbSet<ConceptTree> ConceptTrees { get; set; }

    public virtual DbSet<ConceptTreeClass> ConceptTreeClasses { get; set; }

    public virtual DbSet<ConfigCompetenciesLevel> ConfigCompetenciesLevels { get; set; }

    public virtual DbSet<ConfigConceptTreeLevel> ConfigConceptTreeLevels { get; set; }

    public virtual DbSet<ConfigCycleClass> ConfigCycleClasses { get; set; }

    public virtual DbSet<ConfigDomainField> ConfigDomainFields { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleService> RoleServices { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserRolePermission> UserRolePermissions { get; set; }

    public virtual DbSet<VcompetenciesCascade> VcompetenciesCascades { get; set; }

    public virtual DbSet<VcompetenciesCascadeClass> VcompetenciesCascadeClasses { get; set; }

    public virtual DbSet<VcompetenciesCascadeCross> VcompetenciesCascadeCrosses { get; set; }

    public virtual DbSet<VcompetenciesConceptsCascade> VcompetenciesConceptsCascades { get; set; }

    public virtual DbSet<VconceptsCascade> VconceptsCascades { get; set; }

    public virtual DbSet<VconceptsCascadeClass> VconceptsCascadeClasses { get; set; }

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
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.CompetenciesClasses)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK_CompetenciesClass_Competencies");
        });

        modelBuilder.Entity<CompetenciesConceptField>(entity =>
        {
            entity.ToTable("CompetenciesConceptField");

            entity.Property(e => e.Cid).HasColumnName("CId");
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.CompetenciesConceptFields)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK_CompetenciesConceptField_Competencies");
        });

        modelBuilder.Entity<CompetenciesConceptTree>(entity =>
        {
            entity.ToTable("CompetenciesConceptTree");

            entity.Property(e => e.Cid).HasColumnName("CId");
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");

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
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");

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
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.CompetenciesDomains)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK_CompetenciesDomain_Competencies");
        });

        modelBuilder.Entity<CompetenciesStep>(entity =>
        {
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.Step).HasMaxLength(50);
            entity.Property(e => e.StepComment).HasMaxLength(500);
            entity.Property(e => e.StepDate).HasColumnType("datetime");

            entity.HasOne(d => d.CidNavigation).WithMany(p => p.CompetenciesSteps)
                .HasForeignKey(d => d.Cid)
                .HasConstraintName("FK_CompetenciesSteps_Competencies");
        });

        modelBuilder.Entity<CompetenciesVersion>(entity =>
        {
            entity.Property(e => e.CompetenceDetails).HasMaxLength(500);
            entity.Property(e => e.CompetenceName).HasMaxLength(500);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.IdNumber).HasMaxLength(50);
            entity.Property(e => e.VersionDateCreated).HasColumnType("datetime");
        });

        modelBuilder.Entity<Competencies>(entity =>
        {
            entity.HasIndex(e => new { e.CompetenceLevel, e.Id, e.CompetenceParentId }, "_dta_index_Competencies_10_1010102639__K9_K1_K6_3_4");

            entity.Property(e => e.CompetenceDetails).HasMaxLength(500);
            entity.Property(e => e.CompetenceName).HasMaxLength(500);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.IdNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<ConceptTree>(entity =>
        {
            entity.ToTable("ConceptTree", tb => tb.HasTrigger("trginsertlevel"));

            entity.Property(e => e.ConceptDetails).HasMaxLength(500);
            entity.Property(e => e.ConceptName).HasMaxLength(500);
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.IdNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<ConceptTreeClass>(entity =>
        {
            entity.ToTable("ConceptTreeClass");

            entity.Property(e => e.Ctid).HasColumnName("CTId");
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");

            entity.HasOne(d => d.Ct).WithMany(p => p.ConceptTreeClasses)
                .HasForeignKey(d => d.Ctid)
                .HasConstraintName("FK_ConceptTreeClass_ConceptTree");
        });

        modelBuilder.Entity<ConfigCompetenciesLevel>(entity =>
        {
            entity.ToTable("ConfigCompetenciesLevel");
        });

        modelBuilder.Entity<ConfigConceptTreeLevel>(entity =>
        {
            entity.ToTable("ConfigConceptTreeLevel");
        });

        modelBuilder.Entity<ConfigCycleClass>(entity =>
        {
            entity.ToTable("ConfigCycleClass");
        });

        modelBuilder.Entity<ConfigDomainField>(entity =>
        {
            entity.ToTable("ConfigDomainField");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RoleDetails).HasMaxLength(500);
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<RoleService>(entity =>
        {
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

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
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
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

        modelBuilder.Entity<VcompetenciesCascade>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VCompetenciesCascade");

            entity.Property(e => e.CompetenceName1).HasMaxLength(500);
            entity.Property(e => e.CompetenceName2).HasMaxLength(500);
            entity.Property(e => e.CompetenceName3).HasMaxLength(500);
            entity.Property(e => e.CompetenceName4).HasMaxLength(500);
            entity.Property(e => e.Id1).HasColumnName("id1");
            entity.Property(e => e.Id2).HasColumnName("id2");
            entity.Property(e => e.Id3).HasColumnName("id3");
            entity.Property(e => e.Id4).HasColumnName("id4");
        });

        modelBuilder.Entity<VcompetenciesCascadeClass>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VCompetenciesCascadeClass");

            entity.Property(e => e.Classname)
                .HasMaxLength(255)
                .HasColumnName("classname");
            entity.Property(e => e.CompetenceName1).HasMaxLength(500);
            entity.Property(e => e.CompetenceName2).HasMaxLength(500);
            entity.Property(e => e.CompetenceName3).HasMaxLength(500);
            entity.Property(e => e.CompetenceName4).HasMaxLength(500);
            entity.Property(e => e.Cyclename)
                .HasMaxLength(255)
                .HasColumnName("cyclename");
            entity.Property(e => e.Id1).HasColumnName("id1");
            entity.Property(e => e.Id2).HasColumnName("id2");
            entity.Property(e => e.Id3).HasColumnName("id3");
            entity.Property(e => e.Id4).HasColumnName("id4");
        });

        modelBuilder.Entity<VcompetenciesCascadeCross>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VCompetenciesCascadeCross");

            entity.Property(e => e.CompetenceName1).HasMaxLength(500);
            entity.Property(e => e.CompetenceName2).HasMaxLength(500);
            entity.Property(e => e.CompetenceName3).HasMaxLength(500);
            entity.Property(e => e.CompetenceName4).HasMaxLength(500);
            entity.Property(e => e.Id1).HasColumnName("id1");
            entity.Property(e => e.Id2).HasColumnName("id2");
            entity.Property(e => e.Id3).HasColumnName("id3");
            entity.Property(e => e.Id4).HasColumnName("id4");
            entity.Property(e => e.SubCompetenceLevel1).HasColumnName("subCompetenceLevel1");
            entity.Property(e => e.SubCompetenceLevel2).HasColumnName("subCompetenceLevel2");
            entity.Property(e => e.SubCompetenceLevel3).HasColumnName("subCompetenceLevel3");
            entity.Property(e => e.SubCompetenceLevel4).HasColumnName("subCompetenceLevel4");
            entity.Property(e => e.SubCompetenceName1)
                .HasMaxLength(500)
                .HasColumnName("subCompetenceName1");
            entity.Property(e => e.SubCompetenceName2)
                .HasMaxLength(500)
                .HasColumnName("subCompetenceName2");
            entity.Property(e => e.SubCompetenceName3)
                .HasMaxLength(500)
                .HasColumnName("subCompetenceName3");
            entity.Property(e => e.SubCompetenceName4)
                .HasMaxLength(500)
                .HasColumnName("subCompetenceName4");
            entity.Property(e => e.SubCompetenceType1).HasColumnName("subCompetenceType1");
            entity.Property(e => e.SubCompetenceType2).HasColumnName("subCompetenceType2");
            entity.Property(e => e.SubCompetenceType3).HasColumnName("subCompetenceType3");
            entity.Property(e => e.SubCompetenceType4).HasColumnName("subCompetenceType4");
            entity.Property(e => e.Subid1).HasColumnName("subid1");
            entity.Property(e => e.Subid2).HasColumnName("subid2");
            entity.Property(e => e.Subid3).HasColumnName("subid3");
            entity.Property(e => e.Subid4).HasColumnName("subid4");
        });

        modelBuilder.Entity<VcompetenciesConceptsCascade>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VCompetenciesConceptsCascade");

            entity.Property(e => e.CompetenceName1).HasMaxLength(500);
            entity.Property(e => e.CompetenceName2).HasMaxLength(500);
            entity.Property(e => e.CompetenceName3).HasMaxLength(500);
            entity.Property(e => e.CompetenceName4).HasMaxLength(500);
            entity.Property(e => e.Compid).HasColumnName("compid");
            entity.Property(e => e.Compid1).HasColumnName("compid1");
            entity.Property(e => e.Compid2).HasColumnName("compid2");
            entity.Property(e => e.Compid3).HasColumnName("compid3");
            entity.Property(e => e.ConceptName1).HasMaxLength(500);
            entity.Property(e => e.ConceptName2).HasMaxLength(500);
            entity.Property(e => e.ConceptName3).HasMaxLength(500);
            entity.Property(e => e.ConceptName4).HasMaxLength(500);
            entity.Property(e => e.Conid1).HasColumnName("conid1");
            entity.Property(e => e.Conid2).HasColumnName("conid2");
            entity.Property(e => e.Conid3).HasColumnName("conid3");
            entity.Property(e => e.Conid4).HasColumnName("conid4");
        });

        modelBuilder.Entity<VconceptsCascade>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VConceptsCascade");

            entity.Property(e => e.ConceptDetails1).HasMaxLength(500);
            entity.Property(e => e.ConceptDetails2).HasMaxLength(500);
            entity.Property(e => e.ConceptDetails3).HasMaxLength(500);
            entity.Property(e => e.ConceptDetails4).HasMaxLength(500);
            entity.Property(e => e.ConceptName1).HasMaxLength(500);
            entity.Property(e => e.ConceptName2).HasMaxLength(500);
            entity.Property(e => e.ConceptName3).HasMaxLength(500);
            entity.Property(e => e.ConceptName4).HasMaxLength(500);
            entity.Property(e => e.IdNumber1).HasMaxLength(50);
        });

        modelBuilder.Entity<VconceptsCascadeClass>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VConceptsCascadeClass");

            entity.Property(e => e.Classname)
                .HasMaxLength(255)
                .HasColumnName("classname");
            entity.Property(e => e.ConceptName1).HasMaxLength(500);
            entity.Property(e => e.ConceptName2).HasMaxLength(500);
            entity.Property(e => e.ConceptName3).HasMaxLength(500);
            entity.Property(e => e.ConceptName4).HasMaxLength(500);
            entity.Property(e => e.Cyclename)
                .HasMaxLength(255)
                .HasColumnName("cyclename");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
